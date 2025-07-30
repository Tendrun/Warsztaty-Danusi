using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Service;
using PackageTracker.Core.Interfaces.Services;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carriers")]
    public class CarriersController : ControllerBase
    {
        private readonly ICarrierService _carrierService;
        private readonly IMapper _mapper;
        private readonly ILogger<CarriersController> _logger;

        public CarriersController(
        ICarrierService carrierService,
        IMapper mapper,
        ILogger<CarriersController> logger)
        {
            _carrierService = carrierService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult<Carrier>> AddAsync(CreateCarrierDTO entity)
        {
            var 
        }

        public async Task<ActionResult> DeleteAsync(Guid id)
        {

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrier>>> GetAllAsync()
        {
            var carrier = await _carrierService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<Carrier>>(carrier));
        }

        public async Task<ActionResult<Carrier>> GetByEmailAsync(string email)
        {

        }

        public async Task<ActionResult<Carrier>> GetByIdAsync(Guid id)
        {
           
        }

        public async Task<ActionResult<Carrier>> GetByPhoneNumberAsync(string phoneNumber)
        {

        }

        public async Task<ActionResult> UpdateAsync(Carrier entity)
        {

        }

        public async Task<ActionResult> UpdateIsActiveAsync(Guid id, bool isActive)
        {

        }
    }
}
