using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class PlayerStatistic
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int SportId { get; set; }

    public string StatisticType { get; set; }

    public decimal Value { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Player Player { get; set; }

    public virtual Sport Sport { get; set; }
}
