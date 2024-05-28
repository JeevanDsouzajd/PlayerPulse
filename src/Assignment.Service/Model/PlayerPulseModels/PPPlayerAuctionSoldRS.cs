using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPPlayerAuctionSoldRS
    {
        public int PlayerId { get; set; }

        public int TeamId { get; set; }

        public decimal? SellingPrice { get; set; }
    }
}
