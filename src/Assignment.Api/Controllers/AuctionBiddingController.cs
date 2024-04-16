using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Assignment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionBiddingController : ControllerBase
    {
        private readonly IHubContext<BidNotificationHub> _hubContext;

        public AuctionBiddingController(IHubContext<BidNotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //[HttpPost("bid")]
        //public async Task<IActionResult> PlaceBid([FromBody] BidModel bid)
        //{
        //    // Process the bid

        //    // Notify SignalR Hub about the new bid
        //    await _hubContext.Clients.All.SendAsync("ReceiveBidNotification", bid);

        //    return Ok();
        //}
    }

    // SignalR Hub for handling bid notifications
    public class BidNotificationHub : Hub
    {
        // You can add methods here to handle client connections and bid notifications

    }
}
