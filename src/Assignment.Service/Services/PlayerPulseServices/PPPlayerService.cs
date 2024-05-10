using Amazon.Runtime.Internal;
using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPPlayerService
    {
        private readonly PPIDBPlayerRepository _playerRepository;
        private readonly PPIDBSportRepository _sportRepository;
        private readonly PPIDBUserRepository _userRepository;
        private readonly PPIDBAuctionRepository _auctionRepository;
        public PPPlayerService(PPIDBPlayerRepository playerRepository, PPIDBSportRepository sportRepository, PPIDBUserRepository userRepository, PPIDBAuctionRepository auctionRepository)
        {
            _playerRepository = playerRepository;
            _sportRepository = sportRepository;
            _userRepository = userRepository;
            _auctionRepository = auctionRepository;
        }

        public async Task<IEnumerable<PPPlayerRS>> GetAllPlayersAsync()
        {
            var players = await _playerRepository.GetAllPlayersAsync();

            return players.Select(player => new PPPlayerRS
            {
                Id = player.Id,
                PlayerCode = player.PlayerCode,
                Name = player.Name,
                Email = player.Email,
                Gender = player.Gender,
                Mobile = player.Mobile,
                Age = player.Age,
                BasePrice = player.BasePrice,
                Sold = player.Sold,
                UserId = player.UserId,
                CreatedAt = player.CreatedAt,
                UpdatedAt = player.UpdatedAt
            });
        }

        public async Task<PPPlayerRS> GetPlayerByCodeAsync(string playerCode, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);

            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if (player.UserId != tokenUserId)
            {
                throw new ArgumentException("You do not have permission to view this.");
            }

            if (player == null)
            {
                throw new ArgumentException($"Player with playerCode {playerCode} not found");
            }

            return new PPPlayerRS
            {
                Id = player.Id,
                PlayerCode = player.PlayerCode,
                Name = player.Name,
                Email = player.Email,
                Gender = player.Gender,
                Mobile = player.Mobile,
                Age = player.Age,
                BasePrice = player.BasePrice,
                Sold = player.Sold,
                UserId = player.UserId,
                CreatedAt = player.CreatedAt,
                UpdatedAt = player.UpdatedAt
            };
        }

        public async Task<PPPlayerRS> CreatePlayerAsync(PPPlayerRQ playerRequest, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);
            string emailId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            var user = await _userRepository.GetUserByIdAsync(tokenUserId);

            if (user.Id != tokenUserId ) 
            {
                throw new Exception("You are not authorized to perform this action");
            }

            var existingPlayer = await _playerRepository.GetPlayerByCodeAsync(playerRequest.PlayerCode);

            if (existingPlayer != null)
            {
                throw new ArgumentException("Player with this code already exists.");
            }

            var basePrice = playerRequest.BasePrice ?? 10000;

            if (basePrice > 50000)
            {
                throw new ArgumentException("Base price cannot exceed 50,000.");
            }

            var entity = new Player
            {
                PlayerCode = playerRequest.PlayerCode,
                Name = playerRequest.Name,
                Email = emailId,
                Gender = playerRequest.Gender,
                Mobile = playerRequest.Mobile,
                Age = playerRequest.Age,
                BasePrice = basePrice,
                UserId = tokenUserId,
                Sold = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var player = await _playerRepository.CreatePlayerAsync(entity);

            return new PPPlayerRS
            {
                Id = player.Id,
                PlayerCode = player.PlayerCode,
                Name = player.Name,
                Email = player.Email,
                Gender = player.Gender,
                Mobile = player.Mobile,
                Age = player.Age,
                BasePrice = player.BasePrice,
                Sold = player.Sold,
                UserId = player.UserId,
                CreatedAt = player.CreatedAt,
                UpdatedAt = player.UpdatedAt
            };
        }

        public async Task<PPPlayerRS> UpdatePlayerAsync(string playerCode, PPPlayerUpdateRQ playerRequest, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);
            string emailId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            var existingPlayer = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if (existingPlayer == null)
            {
                throw new ArgumentException($"Player with code {playerCode} not found.");
            }

            if (existingPlayer.UserId != tokenUserId)
            {
                throw new ArgumentException("You do not have permission to update the details");
            }

            if (existingPlayer.User.IsVerified == false)
            {
                throw new ArgumentException("Player not yet verified. Please verify first to update details");
            }

            existingPlayer.Name = playerRequest.Name;
            existingPlayer.Email = emailId;
            existingPlayer.Mobile = playerRequest.Mobile;
            existingPlayer.BasePrice = playerRequest.BasePrice;
            existingPlayer.UpdatedAt = DateTime.UtcNow;

            await _playerRepository.UpdatePlayerAsync(existingPlayer);

            return new PPPlayerRS
            {
                Id = existingPlayer.Id,
                PlayerCode = existingPlayer.PlayerCode,
                Name = existingPlayer.Name,
                Email = existingPlayer.Email,
                Gender = existingPlayer.Gender,
                Mobile = existingPlayer.Mobile,
                Age = existingPlayer.Age,
                BasePrice = existingPlayer.BasePrice,
                Sold = existingPlayer.Sold,
                UserId = existingPlayer.UserId,
                CreatedAt = existingPlayer.CreatedAt,
                UpdatedAt = existingPlayer.UpdatedAt
            };
        }

        public async Task DeletePlayerAsync(string playerCode)
        {
            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if (player == null)
            {
                throw new ArgumentException($"Player with code {playerCode} not found.");
            }

            player.IsDeleted = true;

            await _playerRepository.UpdatePlayerAsync(player);
        }


        public async Task AssignSportToPlayerAsync(string playerCode, string sportCode, LevelType level, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);

            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if(player == null)
            {
                throw new ArgumentException("Player not found");
            }

            await _playerRepository.AssignSportToPlayerAsync(playerCode, sportCode, level, tokenUserId);

        }

        public async Task<PPPlayerStatisticsRS> AddPlayerStatisticsAsync(string playerCode, PPPlayerStatisticsRQ playerStatisticRQ, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);

            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            var user = await _userRepository.GetUserByIdAsync(player.UserId);

            if (user.IsVerified == false)
            {
                throw new ArgumentException($"Player not yet verified. Please verify first to update details");
            }

            if (player.UserId != tokenUserId)
            {
                throw new ArgumentException("You do not have permission to update the details");
            }

            var playerSport = await _playerRepository.GetPlayerSportByPlayerIdAsync(player.Id);

            var statistic = await _playerRepository.GetStatisticByStatisticID(playerStatisticRQ.StatisticTypeId);

            if (playerSport.SportId != statistic.SportId)
            {
                throw new ArgumentException("Invalid statistic submission. Statistics can only be added for the player's registered sport.");
            }

            var playerStatistic = new PlayerStatistic
            {
                PlayerId = player.Id,
                StatisticTypeId = playerStatisticRQ.StatisticTypeId,
                Value = playerStatisticRQ.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var playerStatistics = await _playerRepository.AddPlayerStatisticsAsync(playerStatistic);

            return new PPPlayerStatisticsRS
            {
                Id = playerStatistics.Id,
                StatisticTypeId = playerStatisticRQ.StatisticTypeId,
                Value = playerStatistics.Value
            };
        }

        public async Task<IEnumerable<PPPlayerStatisticsRS>> GetPlayerStatisticsByPlayerCodeAsync(string playerCode)
        {
            var statistics =  await _playerRepository.GetPlayerStatisticsByCodeAsync(playerCode);

            return statistics.Select(statistics => new PPPlayerStatisticsRS
            {
                Id = statistics.Id,
                StatisticTypeId = statistics.StatisticTypeId,
                Value = statistics.Value
            });
        }

        public async Task AssignPlayerToAuctionAsync(string playerCode, int auctionId)
        {
            var player = await _playerRepository.GetPlayerByCodeAsync(playerCode);

            if (player == null)
            {
                throw new Exception($"Player with code '{playerCode}' not found.");
            }

            var playerSport = await _playerRepository.GetPlayerSportByPlayerIdAsync(player.Id);

            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);

            if(playerSport.SportId != auction.SportId) 
            {
                throw new ArgumentException("You cannot assign player to this auction since this auction is for a different sport");
            }

            var user = await _userRepository.GetUserByIdAsync(player.UserId);      

            if (user.IsVerified == false)
            {
                throw new ArgumentException($"Player not yet verified. Please verify first to update details");
            }

            var existingAssignment = await _playerRepository.GetPlayersByAuctionIdAndPlayerIdAsync (player.Id, auctionId);

            if (existingAssignment.Count > 0)
            {
                throw new ArgumentException("Player is already assigned to this auction.");
            }

            // Calculate valuation points based on player level and statistics
            var valuationPoints = await CalculateValuationPoints(player.Id);

            var playerAuction = new PlayerAuction
            {
                PlayerId = player.Id,
                AuctionId = auctionId,
                ValuatedPrice = valuationPoints * player.BasePrice,
                SellingPrice = 0,
                IsActive = false
            };

            await _playerRepository.CreatePlayerAuctionAsync(playerAuction);

            var playerValuation = new PlayerValuation
            {
                PlayerId = player.Id,
                ValuationPoints = valuationPoints,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _playerRepository.CreatePlayerValuationAsync(playerValuation);

        }

        private async Task<decimal> CalculateValuationPoints(int playerId)
        {
            var playerSport = await _playerRepository.GetPlayerSportByPlayerIdAsync(playerId);

            var level = await _playerRepository.GetLevelById(playerSport.LevelId);
            var levelName = level.Name;

            var playerStatistics = await _playerRepository.GetPlayerStatisticsByPlayerIdAsync(playerId);

            decimal levelPoints = levelName switch
            {
                LevelType.Beginner => 5,
                LevelType.Intermediate => 10,
                LevelType.Advanced => 20,
                _ => 0, // Handling unexpected level values
            };

            decimal statPoints = 0;
            foreach (var stat in playerStatistics)
            {
                statPoints += stat.Value * GetStatWeight(stat.StatisticType.SportId, stat.StatisticType.StatisticType);
            }

            var totalPoints = levelPoints + statPoints;
            totalPoints = Math.Floor(totalPoints / 10) * 10;

            return totalPoints;
        }

        private decimal GetStatWeight(int sportId, string statType)
        {
            switch (sportId)
            {
                case 1002: // Football (FB)
                    switch (statType)
                    {
                        case "Goals":
                            return 0.2m;
                        case "Assists":
                            return 0.1m;
                        case "RedCards":
                            return -0.5m;
                        default:
                            return 0.2m;
                    }
                case 1003: // Basketball (BB)
                    switch (statType)
                    {
                        case "PointsScored":
                        case "Assists":
                            return 0.2m;
                        case "Steals":
                            return 0.1m;
                        default:
                            return 0.2m;
                    }
                case 1004: // Tennis (TN)
                    switch (statType)
                    {
                        case "Aces":
                            return 0.2m;
                        case "GamesWon":
                            return 0.1m;
                        case "DoubleFaults":
                            return -0.5m;
                        default:
                            return 0.2m;
                    }
                case 1005: // Cricket (CR)
                    switch (statType)
                    {
                        case "RunsScored":
                        case "WicketsTaken":
                            return 0.2m;
                        case "BattingAverage":
                        case "BowlingAverage":
                            return 0.1m;
                        default:
                            return 0.2m;
                    }
                case 1006: // Volleyball (VB)
                    switch (statType)
                    {
                        case "PointsScored":
                            return 0.2m;
                        case "Blocks":
                        case "Digs":
                            return 0.1m;
                        default:
                            return 0.2m;
                    }
                default:
                    return 0.2m;
            }
        }

        public async Task<IEnumerable<PPPlayerAuctionRS>> GetSoldPlayersByAuctionIdAsync(int auctionId)
        {
            var players = await _playerRepository.GetPlayersByAuctionIdAsync(auctionId);
            var soldPlayers =  players.Where(p => p.IsSold).ToList();

            return soldPlayers.Select(soldPlayers => new PPPlayerAuctionRS
            {
                Id = soldPlayers.Id,
                PlayerId = soldPlayers.PlayerId,
                AuctionId = soldPlayers.AuctionId,
                ValuatedPrice = soldPlayers.ValuatedPrice,
                SellingPrice = soldPlayers.SellingPrice
            });
        }

        public async Task<IEnumerable<PPPlayerAuctionRS>> GetUnsoldPlayersByAuctionIdAsync(int auctionId)
        {
            var players = await _playerRepository.GetPlayersByAuctionIdAsync(auctionId);
            var unsoldPlayers =  players.Where(p => !p.IsSold).ToList();

            return unsoldPlayers.Select(unsoldPlayers => new PPPlayerAuctionRS
            {
                Id = unsoldPlayers.Id,
                PlayerId = unsoldPlayers.PlayerId,
                AuctionId = unsoldPlayers.AuctionId,
                ValuatedPrice = unsoldPlayers.ValuatedPrice,
                SellingPrice = unsoldPlayers.SellingPrice
            });
        }

    }
}
