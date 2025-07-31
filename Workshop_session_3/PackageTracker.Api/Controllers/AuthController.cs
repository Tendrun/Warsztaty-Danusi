using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.DTOs.Auth;
using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.Interfaces.Service;
using PackageTracker.Core.Services;
using Serilog;
using static Dapper.SqlMapper;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("Login")]
        public async Task<ActionResult<TokenJwtResponseDto>> Login(LoginDTO loginDTO)
        {
            return await userService.Login(loginDTO);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(CreateUserDTO entity)
        {
            Log.Debug("Test");
            await userService.AddAsync(entity);
            return Ok();
        }
    }
}
