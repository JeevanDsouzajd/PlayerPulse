using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class TeamPlayer
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int PlayerId { get; set; }

    public DateTime ContractStartDate { get; set; }

    public DateTime ContractEndDate { get; set; }

    public decimal? PurchasedAmount { get; set; }

    public int? AuctionId { get; set; }

    public virtual Player Player { get; set; }

    public virtual Team Team { get; set; }

    public virtual Auction Auction { get; set; }
}
