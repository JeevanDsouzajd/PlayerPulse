using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Api.Controllers
{
    [Route("/sports")]
    [ApiController]
    public class PPSportController : ControllerBase
    {
        private readonly PPSportService _sportService;

        public PPSportController(PPSportService sportService)
        {
            _sportService = sportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sport>>> GetAllSports()
        {
            try
            {
                var sports = await _sportService.GetAllSportsAsync();
                return Ok(new { StatusCode = 200, Message = "Sports Fetched Successfully", Sports = sports });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{sportCode}/statistics")]
        public async Task<ActionResult<IEnumerable<SportStatistic>>> GetAllStatisticsBySportCode(string sportCode)
        {
            try
            {
                var statistics = await _sportService.GetAllStatisticsAsync(sportCode);
                return Ok(new { StatusCode = 200, Message = "Statistics Fetched Successfully", statistics });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{sportCode}/categories")]
        public async Task<ActionResult<IEnumerable<SportStatistic>>> GetAllCategoriesBySportCode(string sportCode)
        {
            try
            {
                var statistics = await _sportService.GetAllCategoriesAsync(sportCode);
                return Ok(new { StatusCode = 200, Message = "Statistics Fetched Successfully", statistics });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
