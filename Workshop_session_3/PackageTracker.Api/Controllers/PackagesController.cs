using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/packages")]
    public class PackagesController : ControllerBase
    {
        /*
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpPost("debug-update-status")]
        public async Task<IActionResult> DebugUpdateStatus(int id, string status)
        {
            await _packageService.UpdateStatusAsync(id, status, "Test z debuggera");
            return Ok();
        }
        */
    }
}
