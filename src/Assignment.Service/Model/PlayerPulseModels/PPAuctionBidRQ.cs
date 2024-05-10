using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionBidRQ
    {
        public int AuctionId { get; set; }

        public string TeamCode { get; set; }

        public string PlayerCode { get; set; }
    }
}
