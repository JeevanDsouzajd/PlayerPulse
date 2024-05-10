using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Repository.PlayerPulseRepository
{
    public class PPPlayerRepository : PPIDBPlayerRepository
    {
        private readonly PlayerPulseContext _dbContext;

        public PPPlayerRepository(PlayerPulseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _dbContext.Players.ToListAsync();
        }

        public async Task<Player> GetPlayerByCodeAsync(string playerCode)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.PlayerCode == playerCode);
        }

        public async Task<int> GetPlayerIdByCodeAsync(string playerCode)
        {
            var player = await _dbContext.Players.FirstOrDefaultAsync(p => p.PlayerCode == playerCode);
            return player.Id;
        }

        public async Task<List<PlayerStatistic>> GetPlayerStatisticsByCodeAsync(string playerCode)
        {
            var playerId = await GetPlayerIdByCodeAsync(playerCode);
            return await _dbContext.PlayerStatistics.Where(p => p.PlayerId == playerId).ToListAsync();
        }

        public async Task<Player> CreatePlayerAsync(Player player)
        {
            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _dbContext.Entry(player).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task AssignSportToPlayerAsync(string playerCode, string sportCode, LevelType levelType, int tokenId)
        {
            var player = await _dbContext.Players.FirstOrDefaultAsync(p => p.PlayerCode == playerCode);

            var sport = await _dbContext.Sports.FirstOrDefaultAsync(s => s.SportCode == sportCode);          

            var level = await _dbContext.Levels.FirstOrDefaultAsync(l => l.Name == levelType);


            if (player.UserId != tokenId)
            {
                throw new ArgumentException("You do not have the permission to assign sports to this player.");
            }
            if (player == null)
            {
                throw new ArgumentException($"Player with code {playerCode} not found.");
            }
            if (sport == null)
            {
                throw new ArgumentException($"Sport with code {sportCode} not found.");
            }
            if (level == null)
            {
                throw new ArgumentException($"Level with name {levelType} not found.");
            }

            var existingSport = await _dbContext.PlayerSports.FirstOrDefaultAsync(ps => ps.PlayerId == player.Id);

            if (existingSport != null)
            {
                throw new ArgumentException("Player is already assigned to a sport. A player can only be assigned to one sport.");
            }


            var playerSport = new PlayerSport
            {
                PlayerId = player.Id,
                SportId = sport.Id,
                LevelId = level.Id,
            };

            _dbContext.PlayerSports.Add(playerSport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PlayerSport> GetPlayerSportByPlayerIdAsync(int playerId)
        {
            return await _dbContext.PlayerSports
                .FirstOrDefaultAsync(ps => ps.PlayerId == playerId);
        }

        public async Task<List<PlayerStatistic>> GetPlayerStatisticsByPlayerIdAsync(int playerId)
        {
            return await _dbContext.PlayerStatistics
                .Include(ps => ps.StatisticType)
                .Where(ps => ps.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<PlayerStatistic> AddPlayerStatisticsAsync(PlayerStatistic playerStatistic)
        {
            _dbContext.PlayerStatistics.Add(playerStatistic);
            await _dbContext.SaveChangesAsync();
            return playerStatistic;
        }

        public async Task CreatePlayerAuctionAsync(PlayerAuction playerAuction)
        {
            await _dbContext.PlayerAuctions.AddAsync(playerAuction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Level> GetLevelById(int levelId)
        {
            return await _dbContext.Levels.FirstOrDefaultAsync(l => l.Id == levelId);
        }

        public async Task<SportStatistic> GetStatisticByStatisticID(int statisticId)
        {
            return await _dbContext.SportStatistics.FirstOrDefaultAsync(l => l.Id == statisticId);
        }

        public async Task<List<PlayerAuction>> GetPlayersByAuctionIdAsync(int auctionId)
        {
            return await _dbContext.PlayerAuctions
                .Where(pa => pa.AuctionId == auctionId)
                .ToListAsync();
        }

        public async Task<List<PlayerAuction>> GetPlayersByAuctionIdAndPlayerIdAsync(int playerId, int auctionId)
        {
            return await _dbContext.PlayerAuctions
                .Where(pa => pa.AuctionId == auctionId && pa.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task CreatePlayerValuationAsync(PlayerValuation playerValuation)
        {
            await _dbContext.PlayerValuations.AddAsync(playerValuation);
            await _dbContext.SaveChangesAsync();
        }
    }
}
