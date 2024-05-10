using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class Level
{
    public int Id { get; set; }

    public LevelType Name { get; set; }

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();
}

public enum LevelType
{
    Beginner,
    Intermediate,
    Advanced
}
