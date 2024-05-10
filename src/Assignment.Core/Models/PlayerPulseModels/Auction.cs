using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModel;

namespace Assignment.Api.Models.PlayerPulseModels;

public partial class Auction
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string League { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime RegistrationStartTime { get; set; }

    public DateTime RegistrationEndTime { get; set; }

    public StatusType Status { get; set; }

    public bool IsActive { get; set; }

    public BiddingType BiddingMechanism { get; set; }

    public int SportId { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual ICollection<AuctionRule> AuctionRules { get; set; } = new List<AuctionRule>();

    public virtual ICollection<AuctionTeam> AuctionTeams { get; set; } = new List<AuctionTeam>();

    public virtual ICollection<PlayerAuction> PlayerAuctions { get; set; } = new List<PlayerAuction>();

    public virtual Sport Sport { get; set; }
}

public enum StatusType
{
    Upcoming, InProgress, Completed
}

public enum BiddingType
{
    OpenBid, SealedBid
}