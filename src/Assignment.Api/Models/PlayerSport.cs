using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class PlayerSport
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int SportId { get; set; }

    public int LevelId { get; set; }

    public virtual Level Level { get; set; }

    public virtual Player Player { get; set; }

    public virtual Sport Sport { get; set; }
}
