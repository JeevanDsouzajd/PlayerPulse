using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBAuctionBidRepository
    {
        Task UpdateAuctionAsync(Auction auction);

        Task<PlayerAuction> GetPlayerAuctionDetailByPlayerCodeAsync(string playerCode);

        Task UpdatePlayerAuctionDetailAsync(PlayerAuction playerAuction);

        Task<Team> GetTeamByTeamCodeAsync(string teamCode);

        Task<PlayerAuction> GetPlayerAuctionByPlayerCodeAsync(string playerCode, int auctionId);

        Task CreateBidAsync(AuctionBid bid);

        Task UpdateBidAsync(AuctionBid bid);

        Task UpdateBidJobIdForPlayerAsync(int playerId, int? newJobId);

        Task<AuctionBid> GetBidByPlayerAndAuctionAsync(int auctionId, string playerCode);

        Task<AuctionRule> GetMinimumBidIncrementForAuctionAsync(int auctionId);

        Task<AuctionBid> GetLatestBidByAuctionAsync(string playerCode);

        Task<List<AuctionBid>> GetAllBidsByPlayerAndAuctionAsync(int auctionId, string playerCode);

        Task UpdatePlayerAuctionAsync(PlayerAuction playerAuction);

        Task<AuctionBid> GetLatestBidByPlayerAuctionAndTeamAsync(int auctionId, string playerCode, int teamId);

        Task<AuctionBid> GetLatestBidByPlayerAndAuctionAsync(int auctionId, string playerCode);

        Task<AuctionBid> GetJobIdByPlayerId(string playerCode);

        Task<AuctionTeam> GetAuctionTeam(int auctionId, int teamId);

        Task UpdateTeamAuctionAsync(AuctionTeam teamAuction);

        Task<IEnumerable<AuctionBid>> GetActiveBidsByAuctionIdAsync(int auctionId);

        Task<PlayerAuction> GetActivePlayerAuctionAsync();
    }
}
