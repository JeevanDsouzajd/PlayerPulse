using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class TeamUser
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Team Team { get; set; }

    public virtual User User { get; set; }
}
