using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.DTOs.Auth;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.Interfaces.Service;

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
    }
}
