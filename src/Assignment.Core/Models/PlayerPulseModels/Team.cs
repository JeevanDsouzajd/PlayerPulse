using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModel;

namespace Assignment.Api.Models.PlayerPulseModels;

public partial class Team
{
    public int Id { get; set; }

    public string TeamCode { get; set; }

    public string Name { get; set; }

    public byte[] Logo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual ICollection<AuctionTeam> AuctionTeams { get; set; } = new List<AuctionTeam>();

    public virtual ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
