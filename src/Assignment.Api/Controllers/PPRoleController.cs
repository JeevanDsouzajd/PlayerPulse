using Assignment.Api.Models;
using Assignment.Api.Models.PlayerPulseModel;
using Assignment.Service.Model.PlayerPulseModels;
using Assignment.Service.Services.PlayerPulseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



namespace Assignment.Api.Controllers
{
    [Route("/roles")]
    [ApiController]
    public class PPRoleController : ControllerBase
    {
        private readonly PPRoleService _roleService;

        public PPRoleController(PPRoleService roleService)
        {
            _roleService = roleService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerPulseRole>>> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(new { StatusCode = 200, Message = "Roles Fetched Successfully", roles });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error",
                    Error = ex.Message
                });
            }
        }
    }
}

