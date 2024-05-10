using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerAuctionRS
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int AuctionId { get; set; }

        public decimal? ValuatedPrice { get; set; }

        public decimal? SellingPrice { get; set; }
    }
}
