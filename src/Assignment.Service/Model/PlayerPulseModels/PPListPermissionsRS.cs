using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Model.PlayerPulseModels
{
    public class PPListPermissionsRS
    {

        public int RoleId { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public List<PPPermission> Permissions { get; set; }

        public class PPPermission
        {
            public int PermissionId { get; set; }
            public string PermissionName { get; set; }
        }
    }
}
