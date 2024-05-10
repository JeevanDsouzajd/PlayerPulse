using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Infrastructure.Repository.PlayerPulseRepository
{
    public class PPRoleRepository : PPIDBRoleRepository
    {
        private readonly PlayerPulseContext _context;

        public PPRoleRepository(PlayerPulseContext context)
        {
            _context = context;
        }

        public async Task<List<PlayerPulseRole>> GetAllRolesAsync()
        {
            return await _context.PlayerPulseRoles.ToListAsync();
        }

        public async Task<bool> RoleExistsAsync(int roleId)
        {
            return await _context.PlayerPulseRoles.AnyAsync(role => role.Id == roleId);
        }

    }
}