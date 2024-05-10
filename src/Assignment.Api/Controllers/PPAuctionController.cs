using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Assignment.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class PPAuctionController : ControllerBase
    {
        private readonly PPAuctionService _auctionService;

        public PPAuctionController(PPAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [CustomAuthorize("auction-create")]
        [HttpPost("auction")]
        public async Task<ActionResult<PPAuctionRS>> CreateAuction([Required] BiddingType bidding , [FromBody] PPAuctionRQ auction)
        {
            try
            {
                var createdAuction = await _auctionService.CreateAuctionAsync(bidding, auction);
                return Ok(new { StatusCode = 200, Message = "Auction Added Successfully", Auction = createdAuction});
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


        [HttpGet("auctions")]   
        public async Task<ActionResult<IEnumerable<PPAuctionRS>>> GetAuctions()
        {
            try
            {
                var auctions = await _auctionService.GetAllAuctionsAsync();
                return Ok(new { StatusCode = 200, Message = "Auctions Fetched Successfully", Auctions = auctions});
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


        [HttpGet("auction/{auctionId}")]
        public async Task<ActionResult<PPAuctionRS>> GetAuctionById(int auctionId)
        {
            try
            {
                var auction = await _auctionService.GetAuctionByIdAsync(auctionId);

                if (auction == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Auction not found" });
                }

                return Ok(new { StatusCode = 200, Message = "Auction Fetched Successfully", Auction = auction});

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
        [HttpPost("auction/assign/rule")]
        public async Task<ActionResult<AuctionRule>> AssignRuleToAuction([Required] [FromForm]RuleEnum rule, [FromForm] PPAuctionRuleRQ auctionRuleRQ)
        {
            try
            {
                var createdAuctionRule = await _auctionService.AssignRuleToAuctionAsync(rule, auctionRuleRQ);
                return Ok(new { StatusCode = 200, Message = "Auction Rule Mapped Successfully", Auction = createdAuctionRule });
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
        [HttpGet("auction/{auctionId}/rules")]
        public async Task<ActionResult<AuctionRule>> GetAuctionRuleById(int auctionId)
        {
            try
            {
                var auctionRule = await _auctionService.GetAuctionRuleByIdAsync(auctionId);

                if (auctionRule == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "Auction not found" });
                }

                return Ok(new { StatusCode = 200, Message = "Auction Rules fetched Successfully", Auction = auctionRule });
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
