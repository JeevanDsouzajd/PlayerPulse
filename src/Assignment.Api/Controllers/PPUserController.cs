using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Api.Controllers
{
    [Route("/user/")]
    [ApiController]
    public class PPUserController : ControllerBase
    {
        private readonly PPUserService _userService;
        private readonly AuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public PPUserController(PPUserService userService, AuthService authService, IHttpContextAccessor accessor)
        {
            _userService = userService;
            _authService = authService;
            _httpContextAccessor = accessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        [CustomAuthorize("user-manage")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerPulseUser>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(new { StatusCode = 200, Message = "Users Fetched Successfully", users });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("user-view")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<PPUserRS>> GetUserById(int userId)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var user = await _userService.GetUserByIdAsync(userId, decyptedtoken);
                if (user == null)
                {
                    return StatusCode(200, new { StatusCode = 404, Message = "User not found" });
                }
                return Ok(new { StatusCode = 200, Message = user });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(PPUserRQ user)
        {
            try
            {
                var userDetails = await _userService.CreateUserAsync(user);
                return Ok(new { StatusCode = 200, Message = "User Registered Successfully", userDetails });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> GenerateToken([FromBody] PPUserLoginRQ userLoginRQ)
        {
            try
            {
                var user = await _userService.GetUserByEmailAndPasswordAsync(userLoginRQ.Email, userLoginRQ.Password);

                if (user == null)
                {
                    return Ok(new { StatusCode = 404, Message = "User Not Found"});
                }

                if (user.IsVerified == false)
                {
                    return StatusCode(400, new { StatusCode = 400, Message = "You have to verify first to generate a token"});
                }

                var accessToken = await _userService.GenerateJwtToken(userLoginRQ.Email);
                return Ok(new { Token = accessToken });

            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [CustomAuthorize("user-view")]
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(int userId, [FromBody] PPUserUpdateRQ user)
        {
            try
            {
                var token = _session.GetString("AccessToken");
                var decyptedtoken = await _authService.DecryptJwt(token);

                var updatedUser = await _userService.UpdateUserAsync(userId, user, decyptedtoken);
                return Ok(new { StatusCode = 200, Message = "User Updated Successfully", updatedUser });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [CustomAuthorize("user-delete")]
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                return Ok(new { StatusCode = 200, Message = "User Deleted Successfully", userId });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }


        [HttpPatch("{userId}/validateuser")]
        public async Task<ActionResult> ValidateUserAsync(int userId, ValidateUserRQ validateUserRequest)
        {
            try
            {

                var user = await _userService.GetUserByIdAsyncWithoutToken(userId);

                if (user == null)
                {
                    return Ok(new { StatusCode = 404, Message = "User Not Found" });
                }

                var validationResult = await _userService.ValidateUserAsync(userId, validateUserRequest.OTP);

                if (validationResult)
                {
                    return Ok(new { StatusCode = 200, Message = "User validated successfully" });
                }

                else
                {
                    return BadRequest(new { StatusCode = 400, Message = "Invalid OTP" });
                }
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { StatusCode = 400, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { StatusCode = 500, Message = "Internal Server Error", Error = ex.Message });
            }
        }
    }
}
