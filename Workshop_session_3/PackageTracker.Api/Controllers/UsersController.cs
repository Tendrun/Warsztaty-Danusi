using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.Interfaces.Service;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


    }
}
