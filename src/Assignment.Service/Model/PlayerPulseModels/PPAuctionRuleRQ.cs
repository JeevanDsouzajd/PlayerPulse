using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionRuleRQ
    {
        [Required]
        public int AuctionId { get; set; }

        [Required]
        public decimal RuleValue { get; set; }
    }
}
