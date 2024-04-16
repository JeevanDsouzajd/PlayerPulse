using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class Auction
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string League { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime RegistrationStartTime { get; set; }

    public DateTime RegistrationEndTime { get; set; }

    public string Status { get; set; }

    public bool? IsActive { get; set; }

    public string BiddingMechanism { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual ICollection<AuctionRule> AuctionRules { get; set; } = new List<AuctionRule>();

    public virtual ICollection<AuctionTeam> AuctionTeams { get; set; } = new List<AuctionTeam>();

    public virtual ICollection<PlayerAuctionStatus> PlayerAuctionStatuses { get; set; } = new List<PlayerAuctionStatus>();
}
