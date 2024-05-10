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
    public class PPUserRepository : PPIDBUserRepository
    {
        private readonly PlayerPulseContext _context;

        public PPUserRepository(PlayerPulseContext context)
        {
            _context = context;
        }

        public async Task<PlayerPulseUser> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _context.PlayerPulseUsers.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
            return user;
        }

        public async Task<PlayerPulseUser> GetPasswordByEmail(string email)
        {
            return await _context.PlayerPulseUsers.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<List<PlayerPulseUser>> GetAllUsersAsync()
        {

            return await _context.PlayerPulseUsers.ToListAsync();

        }

        public async Task<PlayerPulseUser> GetUserByIdAsync(int userId)
        {

            return await _context.PlayerPulseUsers.FindAsync(userId);

        }

        public async Task CreateUserAsync(PlayerPulseUser user)
        {

            _context.PlayerPulseUsers.Add(user);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateUserAsync(PlayerPulseUser user)
        {

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

        public async Task<bool> ValidateUserAsync(int userId, int otp)
        {
            var user = await _context.PlayerPulseUsers.FindAsync(userId);  
            
            bool isValid = user.Otp == otp;

            if (isValid)
            {
                user.IsVerified = true;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return isValid;
        }

        public async Task<int> GetRoleByEmail(string email)
        {
            var user = await _context.PlayerPulseUsers.FirstOrDefaultAsync(u => u.Email == email);
            return user.RoleId;
        }

        public IEnumerable<PermissionWithAction> GetPermissionsByRole(int roleId)
        {
            return _context.PlayerPulseRoleActionPermissions
                .Where(rap => rap.RoleId == roleId)
                .Include(rap => rap.Role)
                .Include(rap => rap.Permission)
                .Include(rap => rap.Action)
                .Select(rap => new PermissionWithAction
                {
                    ActionId = rap.ActionId,
                    ActionName = rap.Action.ActionName,
                    PermissionName = rap.Permission.PermissionName,
                    PermissionId = rap.Permission.Id
                })
                .Distinct()
                .ToList();

        }

        public async Task<PlayerPulseUser> GetUserByEmail(string email)
        {
            return await _context.PlayerPulseUsers.FirstOrDefaultAsync(user => user.Email == email);
        }
    }
}
