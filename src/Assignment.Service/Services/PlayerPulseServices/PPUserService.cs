using Assignment.Api.Interfaces;
using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
using Jose;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Assignment.Service.Model;
using static Assignment.Service.Model.PlayerPulseModels.PPListPermissionsRS;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using Assignment.Service.Helpers;

namespace Assignment.Service.Services.PlayerPulseServices
{
    public class PPUserService
    {
        private readonly PPIDBUserRepository _userRepository;
        private readonly ISession _session;
        private readonly PPIDBRoleRepository _roleRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly EncryptionHelper encryptionHelper;
        private static readonly Random random = new Random();

        public PPUserService(PPIDBUserRepository userRepository, IHttpContextAccessor accessor, PPIDBRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            httpContextAccessor = accessor;
            _session = httpContextAccessor.HttpContext.Session;
            var encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
            encryptionHelper = new EncryptionHelper(encryptionKey);
            _roleRepository = roleRepository;
        }

        public async Task<PlayerPulseUser> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _userRepository.GetPasswordByEmail(email);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var encryptedPassword = encryptionHelper.EncryptString(password);

            return await _userRepository.GetUserByEmailAndPasswordAsync(email, encryptedPassword);
        }

        public async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var usersDto = users.Select(user => new
            {
                user.Id,
                user.Username,
                user.Email,
                user.RoleId,
                user.CreatedAt,
                user.UpdatedAt
            });

            return usersDto;

        }

        public async Task<object> GetUserByIdAsync(int userId, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            if (user.Id != tokenUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to view this user details");
            }

            return new
            {
                User = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.RoleId,
                    user.CreatedAt,
                    user.UpdatedAt
                }
            };

        }

        public async Task<object> GetUserByIdAsyncWithoutToken(int userId)
        {

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new
            {
                User = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.RoleId,
                    user.CreatedAt,
                    user.UpdatedAt
                }
            };

        }

        public async Task<PPUserRS> CreateUserAsync(PPUserRQ user)
        {

            var otp = GenerateOTP();

            var encryptedPassword = encryptionHelper.EncryptString(user.Password);

            var roleExists = await _roleRepository.RoleExistsAsync(user.RoleId);

            if (!roleExists)
            {
                throw new ArgumentException("RoleId not found. Please provide a valid roleId.");
            }

            var existingUser = await _userRepository.GetUserByEmail(user.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("User with this email already exists. Please try a different email address.");
            }

            var entity = new PlayerPulseUser
            {
                Username = user.Username,
                Email = user.Email,
                Password = encryptedPassword,
                RoleId = user.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Otp = otp,
                IsVerified = false,
                IsActive = true
            };

            await _userRepository.CreateUserAsync(entity);

            await SendEmailAsync(entity.Email, $"Hello {user.Username},<br><br> Welcome to PlayerPulse. <br><br> This is your generated UserId : {entity.Id} <br><br> This is your generated OTP : {otp}", "Welcome to PlayerPulse");

            var response = new PPUserRS
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                RoleId = entity.RoleId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            return response;

        }

        public async Task<PPUserUpdateRS> UpdateUserAsync(int userId, PPUserUpdateRQ user, string token)
        {
            var otp = GenerateOTP();
            var encryptedPassword = encryptionHelper.EncryptString(user.Password);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaim = tokenHandler.ReadToken(token) as JwtSecurityToken;
            string UserId = tokenClaim.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            int tokenUserId = Convert.ToInt32(UserId);

            var existingUser = await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new ArgumentException($"User with ID {userId} not found.");
            }

            if (existingUser.IsVerified == false)
            {
                throw new ArgumentException("You have to verify first");
            }

            if(existingUser.Id != tokenUserId)
            {
                throw new ArgumentException("You do not have permission to update the details");
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Password = encryptedPassword;
            existingUser.UpdatedAt = DateTime.UtcNow;
            existingUser.IsVerified = false;
            existingUser.Otp = otp;

            await _userRepository.UpdateUserAsync(existingUser);

            await SendEmailAsync(user.Email, $"Hello {user.Username},<br><br> Welcome to PlayerPulse. <br><br> This is your generated OTP : {otp}", "Welcome to PlayerPulse");


            return new PPUserUpdateRS
            {
                Username = existingUser.Username,
                Email = existingUser.Email,
                UpdatedAt = existingUser.UpdatedAt
            };

        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            user.IsActive = false;

            await _userRepository.UpdateUserAsync(user);

        }

        public async Task<bool> ValidateUserAsync(int userId, int otp)
        {

            bool isValid = await _userRepository.ValidateUserAsync(userId, otp);

            return isValid;

        }

        public async Task SendEmailAsync(string toAddr, string mailbody, string subject)
        {
            MailMessage message = new MailMessage("jdjeevan26@gmail.com", toAddr);
            message.Subject = subject;
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("jdjeevan26@gmail.com", "ebfc vqqa ohvu tldo");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            client.Send(message);
        }

        private int GenerateOTP()
        {
            const int otpLength = 4;
            const int minDigit = 0;
            const int maxDigit = 9;

            StringBuilder otpBuilder = new StringBuilder();
            for (int i = 0; i < otpLength; i++)
            {
                otpBuilder.Append(random.Next(minDigit, maxDigit + 1));
            }
            return int.Parse(otpBuilder.ToString());
        }


        public async Task<string> GenerateJwtToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Secret")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
             new Claim(JwtRegisteredClaimNames.Sub, Environment.GetEnvironmentVariable("Subject")),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            };

            var roleId = await _userRepository.GetRoleByEmail(email);

            claims.Add(new Claim("email", email));

                var userPermissions = GetPermissionsByRole(roleId);

                foreach (var action in userPermissions)
                {
                    foreach (var permission in action.Permissions)
                    {
                        claims.Add(new Claim("permissions", $"{action.ActionName}::{permission.PermissionName}"));
                    }
                }

            var user = await _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                claims.Add(new Claim("UserId", user.Id.ToString()));

            }

            var token = new JwtSecurityToken(
                Environment.GetEnvironmentVariable("ValidIssuer"),
                Environment.GetEnvironmentVariable("ValidIssuer"),
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var encryptedJwt = EncryptJwt(token);
            return encryptedJwt;
        }

        public string EncryptJwt(JwtSecurityToken token)
        {
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var payloadJson = JsonConvert.SerializeObject(tokenString);
            var certificatePath = Environment.GetEnvironmentVariable("CertificatePath");

            X509Certificate2 certWithPublicKey = new X509Certificate2(certificatePath);
            RSA rsaPublicKey = certWithPublicKey.GetRSAPublicKey();

            string encryptedJwt = JWT.Encode(payloadJson, rsaPublicKey, JweAlgorithm.RSA_OAEP, JweEncryption.A256GCM);
            return encryptedJwt;
        }

        public IEnumerable<ListPermissionsRS> GetPermissionsByRole(int roleId)
        {
            var permissions = _userRepository.GetPermissionsByRole(roleId);
            if (!permissions.Any())
            {
                throw new KeyNotFoundException($"{roleId} has no permissions or roleId is invalid");
            }
            var groupedPermissions = permissions.GroupBy(permission => permission.ActionName);
            var rs = groupedPermissions.Select(group => new ListPermissionsRS
            {
                RoleId = roleId,
                ActionName = group.Key,
                Permissions = group.Select(permission => new Permission
                {
                    PermissionName = permission.PermissionName,
                    PermissionId = permission.PermissionId
                }).ToList()
            }).Distinct().ToList();
            return rs;
        }

    }
}
