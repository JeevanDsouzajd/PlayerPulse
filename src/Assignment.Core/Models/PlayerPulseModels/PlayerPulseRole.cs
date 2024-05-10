using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerPulseRole
{
    public int Id { get; set; }

    public string RoleName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<PlayerPulseRoleActionPermission> PlayerPulseRoleActionPermissions { get; set; } = new List<PlayerPulseRoleActionPermission>();

    public virtual ICollection<PlayerPulseUser> PlayerPulseUsers { get; set; } = new List<PlayerPulseUser>();
}
