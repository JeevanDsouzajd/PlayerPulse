using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class Level
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<PlayerSport> PlayerSports { get; set; } = new List<PlayerSport>();
}
