using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Infrastructure.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;

namespace Assignment.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class PPPlayerController : ControllerBase
    {
        private readonly PPPlayerService _playerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly AuthService _authService;

        public PPPlayerController(PPPlayerService playerService, IHttpContextAccessor accessor, AuthService authService)
        {
            _playerService = playerService;
            _authService = authService;
            _httpContextAccessor = accessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        [CustomAuthorize("player-view")]
        [HttpGet("players")]
        public async Task<ActionResult<PPPlayerRS>> GetAllPlayers()
        {
            try
            {
                var players = await _playerService.GetAllPlayersAsync();
                return Ok(new { StatusCode = 200, Message = "Players Fetched Successfully", Players = players });
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

        [CustomAuthorize("player-view")]
        [HttpGet("player/{playerCode}")]
        public async Task<ActionResult<PPPlayerRS>> GetPlayerByCode(string playerCode)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var player = await _playerService.GetPlayerByCodeAsync(playerCode, decyptedtoken);
                if (player == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Player not found" });
                }
                return Ok(new { StatusCode = 200, Message = player });
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

        [CustomAuthorize("player-manage")]
        [HttpPost("player")]
        public async Task<ActionResult<PPPlayerRS>> CreatePlayer(PPPlayerRQ playerRequest)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var player = await _playerService.CreatePlayerAsync(playerRequest, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Player Created Successfully", Player = player });
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

        [CustomAuthorize("player-manage")]
        [HttpPut("player/{playerCode}")]
        public async Task<ActionResult<PPPlayerRS>> UpdatePlayer(string playerCode, PPPlayerUpdateRQ playerRequest)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var player = await _playerService.UpdatePlayerAsync(playerCode, playerRequest, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Player Updated Successfully", Player = player });
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

        [CustomAuthorize("player-manage")]
        [HttpDelete("player/{playerCode}")]
        public async Task<ActionResult> DeletePlayer(string playerCode)
        {
            try
            {
                await _playerService.DeletePlayerAsync(playerCode);
                return Ok(new { StatusCode = 200, Message = "Player Deleted Successfully", PlayerCode = playerCode });
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

        [CustomAuthorize("player-manage")]
        [HttpPost("player/assign/sport")]
        public async Task<ActionResult> AssignSportToPlayer([Required] string playerCode, [Required] string sportCode, [Required] LevelType level, [Required] CategoryEnum category)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                await _playerService.AssignSportToPlayerAsync(playerCode, sportCode, level, category, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Sport assigned to player successfully" });
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

        [CustomAuthorize("player-manage")]
        [HttpPost("player/{playerCode}/statistics")]
        public async Task<ActionResult> AddPlayerStatistics(string playerCode, [FromBody] PPPlayerStatisticsRQ playerStatisticRQ)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                await _playerService.AddPlayerStatisticsAsync(playerCode, playerStatisticRQ, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Player statistics added successfully" });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal server error", Error = ex.Message });
            }
        }

        [CustomAuthorize("player-view")]
        [HttpGet("player/{playerCode}/statistics")]
        public async Task<IActionResult> GetPlayerStatisticsByPlayerCode(string playerCode)
        {
            try
            {
                var statistics = await _playerService.GetPlayerStatisticsByPlayerCodeAsync(playerCode);

                if (statistics == null || !statistics.Any())
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Player statistics not found" });
                }

                return Ok(new { StatusCode = 200, Message = statistics });
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


        [CustomAuthorize("player-enable")]
        [HttpPost("player/{playerCode}/assign/auction/{auctionId}")]
        public async Task<ActionResult> AssignPlayerToAuction(string playerCode, int auctionId)
        {
            try
            {
                await _playerService.AssignPlayerToAuctionAsync(playerCode, auctionId);
                return Ok(new { StatusCode = 200, Message = "Player Assigned to Auction Successfully" });
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
