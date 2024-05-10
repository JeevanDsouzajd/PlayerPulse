using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Repository.PlayerPulseRepository
{
    public class PPSportRepository : PPIDBSportRepository
    {
        private readonly PlayerPulseContext _dbContext;

        public PPSportRepository(PlayerPulseContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Sport>> GetAllSportsAsync()
        {
            return await _dbContext.Sports.ToListAsync();
        }
        
        public async Task<Sport> GetSportIdByCodeAsync(string sportCode)
        {
            return await _dbContext.Sports.FirstOrDefaultAsync(p => p.SportCode == sportCode);
        }

        public async Task<List<SportStatistic>> GetAllStatisticsAsync(string sportCode)
        {
            var sport = await GetSportIdByCodeAsync(sportCode);
            var sportId = sport.Id;

            return await _dbContext.SportStatistics.Where(p => p.SportId == sportId).ToListAsync();
        }

    }
}
