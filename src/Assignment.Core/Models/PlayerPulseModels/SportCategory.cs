using Assignment.Api.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;

namespace Assignment.Infrastructure.Models.PlayerPulseModel;

public partial class SportCategory
{
    public int Id { get; set; }

    public int SportId { get; set; }

    public CategoryEnum Name { get; set; }

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();

    public virtual Sport Sport { get; set; }
}

public enum CategoryEnum
{
    Defender,
    Midfielder,
    Forward,
    Goalkeeper,
    PointGuard,
    ShootingGuard,
    SmallForward,
    PowerForward,
    Center,
    SinglesPlayer,
    DoublesPlayer,
    BaselinePlayer,
    Batsman,
    Bowler,
    Allrounder,
    Setter,
    OutsideHitter,
    MiddleBlocker,
    OppositeHitter,
    Libero
}