using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Infrastructure.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class Sport
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string SportCode { get; set; }

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();

    public virtual ICollection<SportStatistic> SportStatistics { get; set; } = new List<SportStatistic>();

    public virtual ICollection<SportCategory> SportCategories { get; set; } = new List<SportCategory>();

    public virtual ICollection<Auction> Auctions { get; set; } = new List<Auction>();
}
