using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionBidRS
    {
        public int AuctionId { get; set; }

        public int PlayerId { get; set; }

        public int TeamId { get; set; }

        public decimal? BidAmount { get; set; }

        public DateTime BidTime { get; set; }

        public bool IsSold { get; set; }
    }
}
