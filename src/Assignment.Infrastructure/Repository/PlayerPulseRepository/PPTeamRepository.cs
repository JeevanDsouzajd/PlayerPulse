using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
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
    public class PPTeamRepository : PPIDBTeamRepository
    {
        private readonly PlayerPulseContext _context;

        public PPTeamRepository(PlayerPulseContext context)
        {
            _context = context;
        }

        public async Task<Team> CreateTeamAsync(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamByCodeAsync(string teamCode)
        {
            return await _context.Teams.FirstOrDefaultAsync(t => t.TeamCode == teamCode);
        }

        public async Task<int> GetTeamIdByCodeAsync(string teamCode)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamCode == teamCode);
            return team.Id;
        }

        public async Task DeleteTeamAsync(Team team)
        {
            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserTeamManagerAsync(int userId, int teamId)
        {
            var userTeamCount = await _context.TeamUsers.CountAsync(tu => tu.UserId == userId);
            if (userTeamCount > 0 && teamId != teamId)
            {
                throw new ArgumentException("A user can only manage one team.");
            }

            return await _context.TeamUsers.AnyAsync(tu => tu.UserId == userId && tu.TeamId == teamId);
        }

        public async Task AssignTeamManager(int teamId, int userId)
        {
            var teamUser = new TeamUser
            {
                TeamId = teamId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.TeamUsers.AddAsync(teamUser);
            await _context.SaveChangesAsync();
        }

        public async Task<AuctionTeam> RegisterTeamForAuctionAsync(AuctionTeam auctionTeam)
        {
            _context.AuctionTeams.Add(auctionTeam);
            await _context.SaveChangesAsync();
            return auctionTeam;
        }

        public async Task<List<AuctionTeam>> GetAllTeamsAuctionDataByAuctionId(int auctionId)
        {
            return await _context.AuctionTeams.Where(a => a.AuctionId == auctionId).ToListAsync();
        }

        public async Task<AuctionTeam> GetTeamAuctionData(string teamCode, int auctionId)
        {
            var teamId = await GetTeamIdByCodeAsync(teamCode);
            return await _context.AuctionTeams.FirstOrDefaultAsync(a => a.AuctionId == auctionId && a.TeamId == teamId);
        }

        public async Task CreateTeamPlayerAsync(TeamPlayer teamPlayer)
        {
            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TeamUser>> GetTeamManagersByUserIdAsync(int userId)
        {
            return await _context.TeamUsers
              .Where(tm => tm.UserId == userId)
              .ToListAsync();
        }

        public async Task<IEnumerable<TeamPlayer>> GetTeamPlayersByTeamCodeAsync(string teamCode)
        {
            var teamId = await GetTeamIdByCodeAsync(teamCode);

            return await _context.TeamPlayers
                .Where(tp => tp.TeamId == teamId)
                .ToListAsync();
        }
    }
}

