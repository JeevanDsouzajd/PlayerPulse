using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class AuctionRule
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public string RuleType { get; set; }

    public decimal? RuleValue { get; set; }

    public virtual Auction Auction { get; set; }
}
