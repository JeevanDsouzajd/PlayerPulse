using System;
using System.Collections.Generic;

namespace Assignment.Api.Models.PlayerPulseModel;

public partial class PlayerPulseAction
{
    public int Id { get; set; }

    public string ActionName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<PlayerPulseRoleActionPermission> PlayerPulseRoleActionPermissions { get; set; } = new List<PlayerPulseRoleActionPermission>();
}
