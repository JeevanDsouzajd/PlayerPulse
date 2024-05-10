using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPTeamPlayerRS
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public string Status { get; set; }

        public DateTime ContractStartDate { get; set; }

        public DateTime ContractEndDate { get; set; }

        public decimal? PurchasedAmount { get; set; }
    }
}
