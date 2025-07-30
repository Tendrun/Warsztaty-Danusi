using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.Interfaces.Service;
using PackageTracker.Core.Interfaces.Services;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carriers")]
    public class CarriersController : ControllerBase
    {
        private readonly ICarrierService _packageService;
        private readonly IMapper _mapper;
        private readonly ILogger<CarriersController> _logger;

        public CarriersController(
        ICarrierService packageService,
        IMapper mapper,
        ILogger<CarriersController> logger)
        {
            _packageService = packageService;
            _mapper = mapper;
            _logger = logger;
        }
    }
}
