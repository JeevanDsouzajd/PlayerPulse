using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;
using Assignment.Infrastructure.Models.PlayerPulseModel;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerSport
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int SportId { get; set; }

    public int LevelId { get; set; }

    public int? SportCategoryId { get; set; }

    public virtual Level Level { get; set; }

    public virtual Player Player { get; set; }

    public virtual Sport Sport { get; set; }

    public virtual SportCategory SportCategory { get; set; }

}
