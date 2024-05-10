using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionTeamRQ
    {
        [Required]
        public string TeamCode { get; set; }

        [Required]
        public int AuctionId { get; set; }
    }
}
