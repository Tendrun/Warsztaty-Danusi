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
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;
        private readonly ILogger<PackagesController> _logger;

        public PackagesController(
            IPackageService packageService,
            IMapper mapper,
            ILogger<PackagesController> logger)
        {
            _packageService = packageService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages()
        {
            var packages = await _packageService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<PackageDto>>(packages));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageDto>> GetPackage(Guid id)
        {
            var package = await _packageService.GetByIdAsync(id);
            if (package == null)
                return NotFound();

            return Ok(_mapper.Map<PackageDto>(package));
        }

        [HttpPost]
        public async Task<ActionResult<PackageDto>> CreatePackage(CreatePackageDto createPackageDto)
        {
            var package = await _packageService.CreateAsync(createPackageDto);
            return CreatedAtAction(
                nameof(GetPackage),
                new { id = package.Id, version = HttpContext.GetRequestedApiVersion().ToString() },
                _mapper.Map<PackageDto>(package));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePackage(Guid id, UpdatePackageDto updatePackageDto)
        {
            await _packageService.UpdateAsync(id, updatePackageDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            await _packageService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateStatusDto updateStatusDto)
        {
            await _packageService.UpdateStatusAsync(id, updateStatusDto.Status, updateStatusDto.Notes);
            return NoContent();
        }

        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<StatusHistoryDto>>> GetStatusHistory(Guid id)
        {
            var history = await _packageService.GetStatusHistoryAsync(id);
            return Ok(_mapper.Map<IEnumerable<StatusHistoryDto>>(history));
        }

        [HttpGet("track/{trackingNumber}")]
        public async Task<ActionResult<PackageDto>> TrackPackage(string trackingNumber)
        {
            var package = await _packageService.GetByTrackingNumberAsync(trackingNumber);
            if (package == null)
                return NotFound();

            return Ok(_mapper.Map<PackageDto>(package));
        }
    }
}
