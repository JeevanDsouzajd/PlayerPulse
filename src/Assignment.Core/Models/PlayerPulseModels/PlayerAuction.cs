using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModels;

public partial class PlayerAuction
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int AuctionId { get; set; }

    public decimal ValuationPoints { get; set; }

    public decimal? ValuatedPrice { get; set; }

    public decimal? SellingPrice { get; set; }

    public bool IsActive { get; set; }

    public PlayerAuctionStatus Status { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Player Player { get; set; }
}

public enum PlayerAuctionStatus
{
    Upcoming, Unsold, Sold
}
