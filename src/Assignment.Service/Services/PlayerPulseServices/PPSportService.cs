using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPSportService
    {
        private readonly PPIDBSportRepository _sportRepository;

        public PPSportService(PPIDBSportRepository sportRepository)
        {
            _sportRepository = sportRepository;
        }

        public async Task<IEnumerable<object>> GetAllSportsAsync()
        {

            var sport = await _sportRepository.GetAllSportsAsync();
            
            var sportDto = sport.Select(sport => new
            {
                sport.Id,
                sport.Name,
                sport.SportCode
            });

            return sportDto;

        }

        public async Task<IEnumerable<object>> GetAllStatisticsAsync(string sportCode)
        {
            var sportId = await _sportRepository.GetSportIdByCodeAsync(sportCode);

            if(sportId == null)
            {
                throw new ArgumentException("Invalid SportCode.");
            }

            var statistics = await _sportRepository.GetAllStatisticsAsync(sportCode);

            var statisticsDto = statistics.Select(statistics => new
            {
                statistics.Id,
                statistics.StatisticType
            });

            return statisticsDto;
        }

        public async Task<IEnumerable<object>> GetAllCategoriesAsync(string sportCode)
        {
            var sportId = await _sportRepository.GetSportIdByCodeAsync(sportCode);

            if (sportId == null)
            {
                throw new ArgumentException("Invalid SportCode.");
            }

            var categories = await _sportRepository.GetAllCategoriesAsync(sportCode);

            var categoriesDto = categories.Select(categories => new
            {
                categories.Id,
                categories.Name
            });

            return categoriesDto;
        }
    }
}
