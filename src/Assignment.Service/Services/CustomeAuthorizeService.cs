using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Assignment.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Assignment.Service.Services
{
    public class CustomAuthorizationService
    {
        private readonly AuthService _authservice;

        public CustomAuthorizationService(AuthService authService)
        {
            _authservice = authService;
        }
        public async Task<bool> IsAuthorized(string token, params string[] requiredPermissions)
        {
            var decryptedToken = await _authservice.DecryptJwt(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Secret"));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            tokenHandler.ValidateToken(decryptedToken, tokenValidationParameters, out SecurityToken validatedToken);
            var claims = (validatedToken as JwtSecurityToken).Claims;

            var permissionsClaims = claims.Where(claim => claim.Type == "permissions").ToList();
            var organizationsClaims = claims.Where(claim => claim.Type == "organizations").ToList();
            var applicationsClaims = claims.Where(claim => claim.Type == "applications").ToList();

            if (permissionsClaims.Any())
            {
                var hasRequiredPermission = permissionsClaims
                    .Any(claim => requiredPermissions.Any(rp => claim.Value.Contains(rp)));

                return hasRequiredPermission;
            }
            else if (organizationsClaims.Any())
            {
                foreach (var orgClaim in organizationsClaims)
                {
                    var orgDataArray = JArray.Parse(orgClaim.Value);

                    foreach (var orgData in orgDataArray)
                    {
                        if (orgData is JObject orgObject && orgObject.ContainsKey("permissions"))
                        {
                            var orgPermissions = orgObject["permissions"].ToObject<List<string>>();
                            if (requiredPermissions.Any(rp => orgPermissions.Contains(rp)))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else if (applicationsClaims.Any())
            {
                foreach (var appClaim in applicationsClaims)
                {
                    var appDataArray = JArray.Parse(appClaim.Value);

                    foreach (var appData in appDataArray)
                    {
                        if (appData is JObject appObject && appObject.ContainsKey("permissions"))
                        {
                            var appPermissions = appObject["permissions"].ToObject<List<string>>();
                            if (requiredPermissions.Any(rp => appPermissions.Contains(rp)))
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }

            return false;
        }

    }

}