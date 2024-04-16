using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class Player
{
    public int Id { get; set; }

    public string PlayerCode { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Gender { get; set; }

    public string Mobile { get; set; }

    public int Age { get; set; }

    public decimal? BasePrice { get; set; }

    public bool? Sold { get; set; }

    public bool? IsVerified { get; set; }

    public string VerifiedBy { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual ICollection<PlayerAuctionStatus> PlayerAuctionStatuses { get; set; } = new List<PlayerAuctionStatus>();

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();

    public virtual ICollection<PlayerValuation> PlayerValuations { get; set; } = new List<PlayerValuation>();

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
}
