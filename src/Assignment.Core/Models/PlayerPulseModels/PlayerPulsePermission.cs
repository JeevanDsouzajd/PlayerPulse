using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerPulsePermission
{
    public int Id { get; set; }

    public string PermissionName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<PlayerPulseRoleActionPermission> PlayerPulseRoleActionPermissions { get; set; } = new List<PlayerPulseRoleActionPermission>();
}
