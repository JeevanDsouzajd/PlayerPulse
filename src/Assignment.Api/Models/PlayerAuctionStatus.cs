using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class PlayerAuctionStatus
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int AuctionId { get; set; }

    public int TeamId { get; set; }

    public decimal ValuatedPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Player Player { get; set; }

    public virtual Team Team { get; set; }
}
