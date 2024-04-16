using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class AuctionBid
{
    public int Id { get; set; }

    public int AuctionId { get; set; }

    public int PlayerId { get; set; }

    public int TeamId { get; set; }

    public decimal? BidAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime BidTime { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsSold { get; set; }

    public virtual Auction Auction { get; set; }

    public virtual Player Player { get; set; }

    public virtual Team Team { get; set; }
}
