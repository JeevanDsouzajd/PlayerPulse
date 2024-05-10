using System;
using System.Collections.Generic;
using Assignment.Api.Models.PlayerPulseModels;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerPulseUser
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? Otp { get; set; }

    public bool IsVerified { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual PlayerPulseRole Role { get; set; }

    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
}
