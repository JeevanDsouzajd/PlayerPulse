using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;


namespace Assignment.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class PPTeamController : ControllerBase
    {
        private readonly PPTeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly AuthService _authService;

        public PPTeamController(PPTeamService teamService, IHttpContextAccessor accessor, AuthService authService)
        {
            _teamService = teamService;
            _httpContextAccessor = accessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _authService = authService;
        }

        [CustomAuthorize("team-create")]
        [HttpPost("team")]
        public async Task<ActionResult<Team>> CreateTeamAsync([FromForm] PPTeamRQ team)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var createdTeam = await _teamService.CreateTeamAsync(team, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Team added successfully", createdTeam });
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

        [CustomAuthorize("team-view")]
        [HttpGet("teams")]
        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeamsAsync()
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAsync();
                return Ok(new { StatusCode = 200, Message = "Teams Fetched Successfully", Teams = teams });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal server error", Error = ex.Message });
            }
        }

        [CustomAuthorize("team-update")]
        [HttpGet("team/{teamCode}")]
        public async Task<ActionResult<Team>> GetTeamByCodeAsync(string teamCode)
        {
            try
            {

                var team = await _teamService.GetTeamByCodeAsync(teamCode);

                if (team == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Team not found" });
                }

                return Ok(new { StatusCode = 200, Message = team });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal server error", Error = ex.Message });
            }
        }

        [CustomAuthorize("team-delete")]
        [HttpDelete("team/{teamCode}")]
        public async Task<IActionResult> DeleteTeamAsync(string teamCode)
        {
            try
            {

                await _teamService.DeleteTeamAsync(teamCode);
                return Ok(new { StatusCode = 200, Message = "Team Deleted Successfully", teamCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal server error", Error = ex.Message });
            }
        }

        [CustomAuthorize("team-create")]
        [HttpPost("team/{teamCode}/assign/user/{userId}")]
        public async Task<IActionResult> AssignTeamManager(string teamCode, int userId)
        {
            try
            {
                await _teamService.AssignTeamManagerAsync(teamCode, userId);

                return Ok(new { StatusCode = 200, Message = "Team Manager assigned successfully" });

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

        [CustomAuthorize("team-manage")]
        [HttpPost("team/{teamCode}/auction/{auctionId}/register")]
        public async Task<ActionResult> RegisterTeamForAuction(string teamCode, int auctionId)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var auctionTeam = await _teamService.RegisterTeamForAuctionAsync(teamCode, auctionId, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Team Registered for the auction Successfully", auctionTeam });
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

        [CustomAuthorize("team-update")]
        [HttpGet]
        [Route("teams/auction/{auctionId}")]
        public async Task<ActionResult<IEnumerable<PPAuctionTeamRS>>> GetAllTeamsAuctionDataByAuctionId(int auctionId)
        {
            try
            {
                var teams = await _teamService.GetAllTeamsAuctionDataByAuctionId(auctionId);
                return Ok(new { StatusCode = 200, Message = "Teams Fetched Successfully", teams });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        
    }
}
