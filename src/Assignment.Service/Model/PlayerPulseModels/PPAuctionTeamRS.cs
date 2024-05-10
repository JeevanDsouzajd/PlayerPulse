using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPAuctionTeamRS
    {
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public int TeamId { get; set; }

        public DateTime RegistrationTime { get; set; }

        public decimal BudgetAmount { get; set; }

        public decimal BalanceAmount { get; set; }

    }
}
