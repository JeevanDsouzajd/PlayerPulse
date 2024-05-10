using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class AuctionTeam
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int TeamId { get; set; }
    
    public DateTime RegistrationTime { get; set; }

    public decimal BudgetAmount { get; set; }

    public decimal BalanceAmount { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Team Team { get; set; }
}
