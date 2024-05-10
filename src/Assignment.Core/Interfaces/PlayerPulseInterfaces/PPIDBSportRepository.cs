using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBSportRepository
    {
        Task<List<Sport>> GetAllSportsAsync();

        Task<Sport> GetSportIdByCodeAsync(string sportCode);
        
        Task<List<SportStatistic>> GetAllStatisticsAsync(string sportCode);
      
    }
}
