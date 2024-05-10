using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerStatistic
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int StatisticTypeId { get; set; }

    public decimal Value { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Player Player { get; set; }

    public virtual SportStatistic StatisticType { get; set; }
}
