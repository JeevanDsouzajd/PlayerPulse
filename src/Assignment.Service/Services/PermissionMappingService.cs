using Assignment.Api.Models.PlayerPulseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assignment.Service.Services
{
    public interface IPermissionMappingService
    {
        string[] GetPermissionsForIdentifier(string identifier);
    }

    public class PermissionMappingService : IPermissionMappingService
    {
        private readonly Dictionary<string, string[]> _permissionMappings = new Dictionary<string, string[]>
    {
        {"app-management", new[] { "appsetting::create", "appsetting::delete" } },
        {"app-permissions-all", new[] { "appsetting::create", "appsetting::delete", "appsetting::view", "appsetting::edit" } },
        {"org-management", new[] { "orgsetting::create", "orgsetting::delete" } },
        {"org-permissions-all", new[] { "orgsetting::create", "orgsetting::delete", "orgsetting::view", "orgsetting::edit" } },
        {"view-roles-permissions", new[] { "user::view" } },
        {"manage-org-users", new[] { "user::create", "user::delete", } },
        {"view-users", new[] { "user::view" } },
        {"manage-app-users", new[] { "user::create", "user::delete", } },
        {"enable-disable-product", new[] { "appsetting::enable", "appsetting::disable" } },

        //PlayerPulse

        {"user-manage", new[] { "Users::view", "Users::create", "Users::edit", "Users::delete" } },
        {"user-view", new[] { "Users::view", "Users::edit" } },
        {"user-delete", new[] { "Users::delete" } },

        {"team-view", new[] { "Teams::view" } },
        {"team-delete", new[] { "Teams::delete" } },
        {"team-create", new[] { "Teams::create" } },
        {"team-manage", new[] { "Players::view", "Auction-participation::view", "Auction-participation::create", "Teams::create" } },
        {"team-update", new[] { "Teams::update", "Teams::view" } },

        {"player-manage", new[] { "Players::create", "Players::edit" } },
        {"player-view", new[] { "Players::view" } },
        {"player-enable", new[] { "Players::enable" } },

        {"auction-create", new[] { "Auction-management::create", "Auction-management::edit", "Auction-management::delete" } },
        {"auction-manage", new[] { "Players::view","Teams::view","Auction-management::view","Auction-management::create","Auction-management::edit","Auction-management::delete", "Auction-participation::view"} },

        {"auction-participation", new[] { "Auction-participation::create", "Auction-participation::view", "Auction-participation::edit" } },
        
    };

        public string[] GetPermissionsForIdentifier(string identifier)
        {
            return _permissionMappings.TryGetValue(identifier, out var permissions) ? permissions : Array.Empty<string>();
        }
    }
}
