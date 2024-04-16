using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class Sport
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();
}
