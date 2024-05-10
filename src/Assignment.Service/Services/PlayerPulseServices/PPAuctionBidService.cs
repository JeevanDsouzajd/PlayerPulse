using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPAuctionBidService
    {
        private readonly PPIDBAuctionBidRepository _auctionBidRepository;
        private readonly PPIDBAuctionRepository _auctionRepository;
        private readonly PPIDBTeamRepository _teamRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly PPIDBPlayerRepository _playerRepository;


        public PPAuctionBidService(PPIDBAuctionBidRepository auctionBidRepository, PPIDBAuctionRepository auctionRepository, PPIDBTeamRepository teamRepository, IBackgroundJobClient backgroundJobClient, PPIDBPlayerRepository playerRepository)
        {
            _auctionBidRepository = auctionBidRepository;
            _auctionRepository = auctionRepository;
            _teamRepository = teamRepository;
            _backgroundJobClient = backgroundJobClient;
            _playerRepository = playerRepository;
        }

        public async Task StartAuctionAsync(int auctionId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new Exception($"Auction with ID '{auctionId}' not found.");
            }

            if (auction.StartTime > DateTime.UtcNow)
            {
                throw new Exception($"Auction with ID '{auctionId}' cannot be started yet. Start time is {auction.StartTime.ToString("yyyy-MM-dd HH:mm:ss UTC")}.");
            }

            auction.IsActive = true;
            auction.Status = StatusType.InProgress;

            await _auctionBidRepository.UpdateAuctionAsync(auction);
        }

        public async Task ActivatePlayerAuctionAsync(string playerCode)
        {
            var activeAuction = await _auctionBidRepository.GetActivePlayerAuctionAsync();

            if (activeAuction != null)
            {
                throw new Exception("Another player auction is already active. Please wait for it to finish before activating a new one.");
            }

            var playerAuction = await _auctionBidRepository.GetPlayerAuctionDetailByPlayerCodeAsync(playerCode);

            if (playerAuction == null)
            {
                throw new Exception($"Player with player code '{playerCode}' not found.");
            }

            playerAuction.IsActive = true;

            await _auctionBidRepository.UpdatePlayerAuctionDetailAsync(playerAuction);


            var jobId = BackgroundJob.Schedule(() => MarkPlayerAsUnsoldAsync(playerAuction.AuctionId, playerCode), TimeSpan.FromSeconds(80));

        }

        public async Task PlaceBidAsync(int auctionId, string playerCode, string teamCode)
        {
            var teamId = await _teamRepository.GetTeamIdByCodeAsync(teamCode);

            var isTeamRegistered = await _auctionBidRepository.GetAuctionTeam(auctionId, teamId);

            if (isTeamRegistered == null)
            {
                throw new ArgumentException("Team is not registered for this auction");
            }

            var team = await _teamRepository.GetTeamByCodeAsync(teamCode);

            if (team == null)
            {
                throw new ArgumentException("Team with this code does not exist.");
            }


            var existingTeamBid = await _auctionBidRepository.GetLatestBidByPlayerAuctionAndTeamAsync(auctionId, playerCode, teamId);

            var latestBid = await _auctionBidRepository.GetLatestBidByPlayerAndAuctionAsync(auctionId, playerCode);

            if (latestBid != null && latestBid.IsSold)
            {
                throw new ArgumentException("Player is already sold.");
            }

            var auctionRule = await _auctionBidRepository.GetMinimumBidIncrementForAuctionAsync(auctionId);
            var auctionRuleValue = auctionRule.RuleValue;

            var playerAuction = await _auctionBidRepository.GetPlayerAuctionByPlayerCodeAsync(playerCode, auctionId);

            if (playerAuction == null || !playerAuction.IsActive)
            {
                throw new ArgumentException($"Player with code '{playerCode}' is not assigned to the auction or is not active.");
            }

            // Validating team balance before placing bid
            var teamAuction = await _auctionBidRepository.GetAuctionTeam(auctionId, teamId);

            if (teamAuction == null || teamAuction.BalanceAmount < (latestBid?.BidAmount ?? 0) + auctionRuleValue)
            {
                throw new ArgumentException("Insufficient balance to place bid. Your bid amount exceeds your current balance.");
            }

            if (existingTeamBid != null && existingTeamBid.TeamId == teamId)
            {
                if (latestBid != null && latestBid.TeamId != teamId)
                {
                    existingTeamBid.BidAmount = latestBid.BidAmount + auctionRuleValue;
                    existingTeamBid.BidTime = DateTime.UtcNow;

                    var existingJobId = await _auctionBidRepository.GetJobIdByPlayerId(playerCode);

                    if (existingJobId != null)
                    {
                        BackgroundJob.Delete(existingJobId.JobId.ToString());
                    }

                    var jobId = BackgroundJob.Schedule(() => StartBiddingCountdownAsync(auctionId, playerCode), TimeSpan.FromSeconds(30));
                    existingTeamBid.JobId = Convert.ToInt32(jobId);

                    await _auctionBidRepository.UpdateBidAsync(existingTeamBid);

                    await _auctionBidRepository.UpdateBidJobIdForPlayerAsync(existingTeamBid.PlayerId, existingTeamBid.JobId);

                }
                else
                {
                    throw new ArgumentException("This team has already placed the latest bid. Wait for another team to bid before bidding again.");
                }
            }
            else
            {
                var newBid = new AuctionBid
                {
                    AuctionId = auctionId,
                    PlayerId = playerAuction.PlayerId,
                    TeamId = teamId,
                    BidAmount = latestBid != null ? latestBid.BidAmount + auctionRuleValue : playerAuction.ValuatedPrice,
                    CreatedAt = DateTime.UtcNow,
                    BidTime = DateTime.UtcNow,
                    IsActive = true,
                    IsSold = false,
                    JobId = null
                };

                var existingJobId = await _auctionBidRepository.GetJobIdByPlayerId(playerCode);

                if(existingJobId != null)
                {
                    BackgroundJob.Delete(existingJobId.JobId.ToString());
                }

                await _auctionBidRepository.CreateBidAsync(newBid);

                var jobId = BackgroundJob.Schedule(() => StartBiddingCountdownAsync(auctionId, playerCode), TimeSpan.FromSeconds(30));
                newBid.JobId = Convert.ToInt32(jobId);

                 await _auctionBidRepository.UpdateBidJobIdForPlayerAsync(newBid.PlayerId, newBid.JobId);
            }                     

        }

        public async Task StartBiddingCountdownAsync(int auctionId, string playerCode)
        {
            Console.WriteLine("BiddingCountdownAsync called!");

            var existingBids = await _auctionBidRepository.GetAllBidsByPlayerAndAuctionAsync(auctionId, playerCode);

            if (existingBids.Any())
            {
                var highestBid = existingBids.OrderByDescending(b => b.BidTime).First();

                if (highestBid.IsSold)
                {
                    return;
                }

                highestBid.IsSold = true;
                await _auctionBidRepository.UpdateBidAsync(highestBid);

                foreach (var bid in existingBids)
                {
                    bid.IsActive = false;
                    await _auctionBidRepository.UpdateBidAsync(bid);
                }

                await MarkPlayerAsSoldAsync(auctionId, playerCode);

            }

        }

        private async Task CreateTeamPlayerAsync(int teamId, int playerId, decimal purchaseAmount)
        {
            var teamPlayer = new TeamPlayer
            {
                TeamId = teamId,
                PlayerId = playerId,
                Status = "Active",
                ContractStartDate = DateTime.UtcNow,
                ContractEndDate = DateTime.UtcNow.AddYears(1),
                PurchasedAmount = purchaseAmount
            };

            await _teamRepository.CreateTeamPlayerAsync(teamPlayer);
        }

        public async Task DeductTeamBalanceAsync(int auctionId, int teamId, decimal bidAmount)
        {
            var teamAuction = await _auctionBidRepository.GetAuctionTeam(auctionId, teamId);

            if (teamAuction != null)
            {
                if (teamAuction.BalanceAmount >= bidAmount)
                {
                    teamAuction.BalanceAmount -= bidAmount;
                    await _auctionBidRepository.UpdateTeamAuctionAsync(teamAuction);
                }
                else
                {
                    throw new ArgumentException($"Insufficient balance for team {teamId} in auction {auctionId}.");
                }
            }
            else
            {
                throw new ArgumentException($"Team {teamId} not registered for auction {auctionId}.");
            }
        }

        public async Task MarkPlayerAsUnsoldAsync(int auctionId, string playerCode)
        {
            var existingBids = await _auctionBidRepository.GetAllBidsByPlayerAndAuctionAsync(auctionId, playerCode);

            if (!existingBids.Any())
            {
                var playerAuction = await _auctionBidRepository.GetPlayerAuctionByPlayerCodeAsync(playerCode, auctionId);

                if (playerAuction != null)
                {
                    playerAuction.IsSold = false;
                    playerAuction.IsActive = false;
                    await _auctionBidRepository.UpdatePlayerAuctionAsync(playerAuction);
                }
            }
        }

        public async Task MarkPlayerAsSoldAsync(int auctionId, string playerCode)
        {
            var playerAuction = await _auctionBidRepository.GetPlayerAuctionByPlayerCodeAsync(playerCode, auctionId);
            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if (playerAuction != null)
            {
                var latestSoldBid = await _auctionBidRepository.GetLatestBidByPlayerAndAuctionAsync(auctionId, playerCode);

                if (latestSoldBid != null && latestSoldBid.IsSold == true)
                {
                    playerAuction.SellingPrice = latestSoldBid.BidAmount;
                    playerAuction.IsSold = true;
                    player.Sold = true;
                    playerAuction.IsActive = false;

                    await CreateTeamPlayerAsync(latestSoldBid.TeamId, latestSoldBid.PlayerId, (decimal)latestSoldBid.BidAmount);
                    await DeductTeamBalanceAsync(auctionId, latestSoldBid.TeamId, (decimal)latestSoldBid.BidAmount);
                    await _auctionBidRepository.UpdatePlayerAuctionAsync(playerAuction);
                    await _playerRepository.UpdatePlayerAsync(player);
                }
                else
                {
                    throw new ArgumentException($"No sold bids found for player with code '{playerCode}' in auctionId '{auctionId}'.");
                }
            }
        }

        public async Task<PPAuctionBidRS> GetCurrentBidAsync(string playerCode)
        {
            var auctionBid = await _auctionBidRepository.GetLatestBidByAuctionAsync(playerCode);

            if (auctionBid == null)
            {
                throw new ArgumentException("No bids found for this player");
            }

            return new PPAuctionBidRS
            {
                AuctionId = auctionBid.AuctionId,
                PlayerId = auctionBid.PlayerId,
                TeamId = auctionBid.TeamId,
                BidAmount = auctionBid.BidAmount,
                BidTime = auctionBid.BidTime,
                IsSold = auctionBid.IsSold
            };
        }

        public async Task EndAuctionAsync(int auctionId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);

            if (auction == null)
            {
                throw new ArgumentException($"Auction with ID '{auctionId}' not found.");
            }

            if (!auction.IsActive)
            {
                throw new ArgumentException($"Auction with ID '{auctionId}' is not currently active.");
            }

            var activeBids = await _auctionBidRepository.GetActiveBidsByAuctionIdAsync(auctionId);

            if (activeBids.Any())
            {
                throw new ArgumentException("Cannot end auction while there are active bids.");
            }

            auction.IsActive = false;
            auction.Status = StatusType.Completed;

            await _auctionBidRepository.UpdateAuctionAsync(auction);
        }
    }
}
