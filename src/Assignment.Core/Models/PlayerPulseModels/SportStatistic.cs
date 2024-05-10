using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class SportStatistic
{
    public int Id { get; set; }

    public int SportId { get; set; }

    public string StatisticType { get; set; }

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();

    public virtual Sport Sport { get; set; }
}
