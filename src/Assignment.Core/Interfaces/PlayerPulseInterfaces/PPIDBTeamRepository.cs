using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBTeamRepository
    {
        Task<Team> CreateTeamAsync(Team team);

        Task<IEnumerable<Team>> GetAllTeamsAsync();

        Task<Team> GetTeamByCodeAsync(string teamCode);

        Task<int> GetTeamIdByCodeAsync(string teamCode);

        Task DeleteTeamAsync(Team team);

        Task<bool> IsUserTeamManagerAsync(int userId, int teamId);

        Task AssignTeamManager(int teamId, int userId);

        Task<AuctionTeam> RegisterTeamForAuctionAsync(AuctionTeam auctionTeam);

        Task<List<AuctionTeam>> GetAllTeamsAuctionDataByAuctionId(int auctionId);

        Task<AuctionTeam> GetTeamAuctionData(string teamCode, int auctionId);

        Task CreateTeamPlayerAsync(TeamPlayer teamPlayer);

        Task<List<TeamUser>> GetTeamManagersByUserIdAsync(int userId);

        Task<TeamUser> GetTeamManagerByTeamId(int teamId);

        Task<IEnumerable<TeamPlayer>> GetTeamPlayersByTeamCodeAndAuctionIdAsync(string teamCode, int auctionId);

        Task<TeamUser> GetTeamUser(int teamId);
    }
}
