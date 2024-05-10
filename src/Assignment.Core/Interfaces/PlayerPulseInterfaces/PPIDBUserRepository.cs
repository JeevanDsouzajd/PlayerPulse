using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Api.Interfaces.PlayerPulseInterfaces
{
    public interface PPIDBUserRepository
    {   
        Task<List<PlayerPulseUser>> GetAllUsersAsync();

        Task<PlayerPulseUser> GetUserByIdAsync(int userId);

        Task<PlayerPulseUser> GetUserByEmailAndPasswordAsync(string email, string password);

        Task<PlayerPulseUser> GetPasswordByEmail(string email);

        Task CreateUserAsync(PlayerPulseUser user);

        Task UpdateUserAsync(PlayerPulseUser user);

        Task<bool> ValidateUserAsync(int userId, int otp);

        Task<int> GetRoleByEmail(string email);

        IEnumerable<PermissionWithAction> GetPermissionsByRole(int roleId);

        Task<PlayerPulseUser> GetUserByEmail(string email);
    }
}
