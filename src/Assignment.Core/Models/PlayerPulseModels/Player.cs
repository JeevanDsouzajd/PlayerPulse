using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModel;

namespace Assignment.Api.Models.PlayerPulseModels;

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

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual ICollection<PlayerAuction> PlayerAuctions { get; set; } = new List<PlayerAuction>();

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

    public virtual PlayerPulseUser User { get; set; }
}
