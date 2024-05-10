using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBRoleRepository
    {
        Task<List<PlayerPulseRole>> GetAllRolesAsync();

        Task<bool> RoleExistsAsync(int roleId);
    }
}
