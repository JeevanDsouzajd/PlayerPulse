using Assignment.Api.Models.PlayerPulseModels;
using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class AuctionRule
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int RuleId { get; set; }

    public decimal RuleValue { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Rule Rule { get; set; }
}
