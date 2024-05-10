using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
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

        public PPAuctionBidController(PPAuctionBidService auctionBidService)
        {
            _auctionBidService = auctionBidService;
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
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("auction-create")]
        [HttpPatch("player/{playerCode}/activate")]
        public async Task<ActionResult> ActivatePlayerAuction(string playerCode)
        {
            try
            {
                await _auctionBidService.ActivatePlayerAuctionAsync(playerCode);
                return Ok(new { StatusCode = 200, Message = "Player Activated Successfully" });
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
                await _auctionBidService.PlaceBidAsync(auctionBid.AuctionId, auctionBid.PlayerCode, auctionBid.TeamCode);
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

    }
}
