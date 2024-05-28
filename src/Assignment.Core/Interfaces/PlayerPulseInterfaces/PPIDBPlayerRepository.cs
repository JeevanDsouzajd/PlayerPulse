using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Infrastructure.Models.PlayerPulseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();

        Task<Player> GetPlayerByCodeAsync(string playerCode);

        Task<int> GetPlayerIdByCodeAsync(string playerCode);

        Task<List<PlayerStatistic>> GetPlayerStatisticsByCodeAsync(string playerCode);

        Task<Player> CreatePlayerAsync(Player player);

        Task AssignSportToPlayerAsync(string playerCode, string sportCode, LevelType levelType, CategoryEnum categoryType, int tokenId);

        Task<List<PlayerSport>> GetPlayerSportByPlayerIdAsync(int playerId);

        Task<List<PlayerStatistic>> GetPlayerStatisticsByPlayerIdAsync(int playerId);

        Task UpdatePlayerAsync(Player player);

        Task<PlayerStatistic> AddPlayerStatisticsAsync(PlayerStatistic playerStatistic);

        Task CreatePlayerAuctionAsync(PlayerAuction playerAuction);

        Task<Level> GetLevelById(int levelId);

        Task<SportStatistic> GetStatisticByStatisticID(int statisticId);

        Task<List<PlayerAuction>> GetPlayersByAuctionIdAsync(int auctionId);

        Task<List<PlayerAuction>> GetPlayersByAuctionIdAndPlayerIdAsync(int playerId, int auctionId);

        Task<PlayerStatistic> GetPlayerStatisticByPlayerIdAndStatisticTypeAsync(int playerId, int statisticTypeId);

    }

}
