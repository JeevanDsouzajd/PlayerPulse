using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Repository.PlayerPulseRepository
{
    public class PPAuctionBidRepository : PPIDBAuctionBidRepository
    {
        private readonly PlayerPulseContext _dbContext;

        public PPAuctionBidRepository(PlayerPulseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAuctionAsync(Auction auction)
        {
            _dbContext.Auctions.Update(auction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PlayerAuction> GetPlayerAuctionDetailByPlayerCodeAsync(string playerCode)
        {
            return await _dbContext.PlayerAuctions.FirstOrDefaultAsync(p => p.Player.PlayerCode == playerCode);
        }

        public async Task UpdatePlayerAuctionDetailAsync(PlayerAuction playerAuction)
        {
            _dbContext.PlayerAuctions.Update(playerAuction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AuctionBid> GetBidByPlayerAndAuctionAsync(int auctionId, string playerCode)
        {
            return await _dbContext.AuctionBids
                .Include(b => b.Team)
                .FirstOrDefaultAsync(b => b.AuctionId == auctionId && b.Player.PlayerCode == playerCode);
        }

        public async Task UpdateBidAsync(AuctionBid bid)
        {
            _dbContext.AuctionBids.Update(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBidJobIdForPlayerAsync(int playerId, int? newJobId)
        {

            var playerBids = await _dbContext.AuctionBids.Where(e => e.PlayerId == playerId).ExecuteUpdateAsync(s => s.SetProperty(t => t.JobId, newJobId));

        }

        public async Task CreateBidAsync(AuctionBid bid)
        {
            await _dbContext.AuctionBids.AddAsync(bid);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PlayerAuction> GetPlayerAuctionByPlayerCodeAsync(string playerCode, int auctionId)
        {
            return await _dbContext.PlayerAuctions
                .Include(pa => pa.Player)
                .FirstOrDefaultAsync(pa => pa.Player.PlayerCode == playerCode && pa.AuctionId == auctionId);
        }

        public async Task<Team> GetTeamByTeamCodeAsync(string teamCode)
        {
            return await _dbContext.Teams.FirstOrDefaultAsync(t => t.TeamCode == teamCode);
        }

        public async Task<AuctionRule> GetMinimumBidIncrementForAuctionAsync(int auctionId)
        {
            return await _dbContext.AuctionRules.FirstOrDefaultAsync(ar => ar.AuctionId == auctionId && ar.RuleId == 1);
        }

        public async Task<AuctionBid> GetLatestBidByAuctionAsync(string playerCode)
        {
            return await _dbContext.AuctionBids
                .OrderByDescending(b => b.BidTime)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(b => b.Player.PlayerCode == playerCode);
        }

        public async Task<List<AuctionBid>> GetAllBidsByPlayerAndAuctionAsync(int auctionId, string playerCode)
        {
            return await _dbContext.AuctionBids
                .Include(b => b.Team)
                .Where(b => b.AuctionId == auctionId && b.Player.PlayerCode == playerCode)
                .ToListAsync();
        }

        public async Task UpdatePlayerAuctionAsync(PlayerAuction playerAuction)
        {
            _dbContext.PlayerAuctions.Update(playerAuction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<AuctionBid> GetLatestBidByPlayerAuctionAndTeamAsync(int auctionId, string playerCode, int teamId)
        {
            return await _dbContext.AuctionBids
                .Where(b => b.AuctionId == auctionId && b.Player.PlayerCode == playerCode && b.TeamId == teamId)
                .OrderByDescending(b => b.BidTime)
                .FirstOrDefaultAsync();
        }

        public async Task<AuctionBid> GetLatestBidByPlayerAndAuctionAsync(int auctionId, string playerCode)
        {
            return await _dbContext.AuctionBids
                .Where(b => b.AuctionId == auctionId && b.Player.PlayerCode == playerCode)
                .OrderByDescending(b => b.BidTime)
                .FirstOrDefaultAsync();
        }

        public async Task<AuctionBid> GetJobIdByPlayerId(string playerCode)
        {
            var jobId = await _dbContext.AuctionBids
                .Where(b => b.Player.PlayerCode == playerCode)
                .FirstOrDefaultAsync();

            return jobId;
        }

        public async Task<AuctionTeam> GetAuctionTeam(int auctionId, int teamId)
        {
            return await _dbContext.AuctionTeams.FirstOrDefaultAsync(at => at.AuctionId == auctionId && at.TeamId == teamId);
        }

        public async Task UpdateTeamAuctionAsync(AuctionTeam teamAuction)
        {
            _dbContext.AuctionTeams.Update(teamAuction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuctionBid>> GetActiveBidsByAuctionIdAsync(int auctionId)
        {
            return await _dbContext.AuctionBids
                .Where(b => b.AuctionId == auctionId && b.IsActive)
                .ToListAsync();
        }

        public async Task<PlayerAuction> GetActivePlayerAuctionAsync()
        {
            return await _dbContext.PlayerAuctions
              .Where(pa => pa.IsActive == true)
              .FirstOrDefaultAsync();
        }
    }
}
