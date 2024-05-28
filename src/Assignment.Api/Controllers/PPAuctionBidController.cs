using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class PPAuctionBidController : ControllerBase
    {
        private readonly PPAuctionBidService _auctionBidService;
        private readonly PPPlayerService _playerService;
        private readonly PPTeamService _teamService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly AuthService _authService;

        public PPAuctionBidController(PPAuctionBidService auctionBidService, IHttpContextAccessor accessor, AuthService authService, PPPlayerService playerService, PPTeamService teamService)
        {
            _auctionBidService = auctionBidService;
            _httpContextAccessor = accessor;
            _session = _httpContextAccessor.HttpContext.Session;
            _authService = authService;
            _playerService = playerService;
            _teamService = teamService;
        }

        [CustomAuthorize("auction-create")]
        [HttpPatch("auction/{auctionId}/start")]
        public async Task<ActionResult> StartAuction(int auctionId)
        {
            try
            {
                await _auctionBidService.StartAuctionAsync(auctionId);
                return Ok(new { StatusCode = 200, Message = "Auction Started Successfully" });
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

        [CustomAuthorize("auction-create")]
        [HttpPatch("auction/{auctionId}/activate/player/{playerCode}")]
        public async Task<ActionResult> ActivatePlayerAuction(string playerCode, int auctionId)
        {
            try
            {
                await _auctionBidService.ActivatePlayerAuctionAsync(playerCode, auctionId);
                return Ok(new { StatusCode = 200, Message = "Player Activated Successfully" });
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

        [CustomAuthorize("auction-participation")]
        [HttpPost("auction/player/bid")]
        public async Task<ActionResult> PlaceBid([FromBody] PPAuctionBidRQ auctionBid)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                await _auctionBidService.PlaceBidAsync(auctionBid.AuctionId, auctionBid.PlayerCode, auctionBid.TeamCode, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "Bid Placed Successfully" });
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

        [CustomAuthorize("auction-participation")]
        [HttpGet("auction/player/{playerCode}/currentbid")]
        public async Task<ActionResult<PPAuctionBidRS>> GetCurrentBidForPlayer(string playerCode)
        {
            try
            {
                var currentBid = await _auctionBidService.GetCurrentBidAsync(playerCode);
                if (currentBid == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Bid not found" });
                }
                return Ok(new { StatusCode = 200, Message = "Latest Bid Fetched Successfully", Bid = currentBid });
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

        [CustomAuthorize("auction-create")]
        [HttpPatch("auction/{auctionId}/end")]
        public async Task<ActionResult> EndAuction(int auctionId)
        {
            try
            {
                await _auctionBidService.EndAuctionAsync(auctionId);
                return Ok(new { StatusCode = 200, Message = "Auction Ended Successfully" });
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
        [HttpGet]
        [Route("auction/{auctionId}/players")]
        public async Task<ActionResult<IEnumerable<PlayerAuction>>> GetPlayersByAuctionId(int auctionId)
        {
            try
            {
                var players = await _playerService.GetPlayersByAuctionIdAsync(auctionId);

                if (players.Count() == 0)
                {
                    return Ok(new { StatusCode = 404, Message = "No players found" });
                }

                return Ok(new { StatusCode = 200, Message = "Players Fetched Successfully", players });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("player-view")]
        [HttpGet]
        [Route("auction/{auctionId}/players/sold")]
        public async Task<ActionResult<IEnumerable<PlayerAuction>>> GetSoldPlayersByAuctionId([FromRoute] int auctionId)
        {
            try
            {
                var players = await _playerService.GetSoldPlayersByAuctionIdAsync(auctionId);

                if (players.Count() == 0)
                {
                    return Ok(new { StatusCode = 404, Message = "No sold players found" });
                }

                return Ok(new { StatusCode = 200, Message = "Sold Players Fetched Successfully", players });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("player-view")]
        [HttpGet]
        [Route("auction/{auctionId}/players/unsold")]
        public async Task<ActionResult<IEnumerable<PlayerAuction>>> GetUnsoldPlayersByAuctionId([FromRoute] int auctionId)
        {
            try
            {
                var players = await _playerService.GetUnsoldPlayersByAuctionIdAsync(auctionId);

                if (players.Count() == 0)
                {
                    return Ok(new { StatusCode = 404, Message = "No unsold players found" });
                }

                return Ok(new { StatusCode = 200, Message = "Unsold Players Fetched Successfully", players });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("team-update")]
        [HttpGet]
        [Route("auction/{auctionId}/team/{teamCode}")]
        public async Task<ActionResult<PPAuctionTeamRS>> GetTeamAuctionData([FromRoute] string teamCode, int auctionId)
        {
            try
            {
                if (string.IsNullOrEmpty(teamCode))
                {
                    return BadRequest(new { StatusCode = 400, Message = "Team code cannot be empty" });
                }

                var team = await _teamService.GetTeamAuctionData(teamCode, auctionId);

                if (team == null)
                {
                    return Ok(new { StatusCode = 404, Message = "Team Detail not found" });
                }
                return Ok(new { StatusCode = 200, Message = "Team Details Fetched Successfully", team });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("team-view")]
        [HttpGet("auction/{auctionId}/team/{teamCode}/players")]
        public async Task<ActionResult<IEnumerable<PPTeamPlayerRS>>> GetTeamRoster(string teamCode, int auctionId)
        {
            try
            {
                var players = await _teamService.GetTeamRosterAsync(teamCode, auctionId);

                if (!players.Any())
                {
                    return Ok(new { StatusCode = 404, Message = "Player Details not found for this team" });
                }

                return Ok(new { StatusCode = 200, Message = "Players Fetched Successfully", players });
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
