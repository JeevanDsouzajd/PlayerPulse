using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerPulseRoleActionPermission
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ActionId { get; set; }

    public int PermissionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual PlayerPulseAction Action { get; set; }

    public virtual PlayerPulsePermission Permission { get; set; }

    public virtual PlayerPulseRole Role { get; set; }
}
