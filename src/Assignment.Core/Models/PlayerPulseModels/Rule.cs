using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModel;

namespace Assignment.Api.Models.PlayerPulseModels;

public partial class Rule
{
    public int Id { get; set; }

    public RuleEnum RuleType { get; set; }

    public virtual ICollection<AuctionRule> AuctionRules { get; set; } = new List<AuctionRule>();
}

public enum RuleEnum
{
    TeamBudget, MaximumPlayers, BidIncrement
}
