﻿using System;
using System.Collections.Generic;

namespace Assignment.Api.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int? RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Role Role { get; set; }

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
