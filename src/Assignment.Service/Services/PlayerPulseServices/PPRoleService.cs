using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Service.Model.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPRoleService
    {
        private readonly PPIDBRoleRepository _roleRepository;

        public PPRoleService(PPIDBRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<object>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var rolesDto = roles.Select(user => new
            {
                user.Id,
                user.RoleName
            });

            return rolesDto;
        }
    }
}
