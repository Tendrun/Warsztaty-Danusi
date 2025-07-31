using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.DTOs.Carrier;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Service;
using PackageTracker.Core.Interfaces.Services;
using static Dapper.SqlMapper;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/carriers")]
    public class CarriersController : ControllerBase
    {
        private readonly ICarrierService _carrierService;
        private readonly ILogger<CarriersController> _logger;

        public CarriersController(
        ICarrierService carrierService,
        ILogger<CarriersController> logger)
        {
            _carrierService = carrierService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<CarrierDTO>> AddAsync(CreateCarrierDTO entity)
        {
            return await _carrierService.AddAsync(entity);
        }

        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _carrierService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarrierDTO>>> GetAllAsync()
        {
            var carriers = await _carrierService.GetAllAsync();
            return Ok(carriers);
        }

        public async Task<ActionResult<CarrierDTO>> GetByEmailAsync(string email)
        {
            var carriers = await _carrierService.GetByEmailAsync(email);
            return Ok(carriers);
        }

        public async Task<ActionResult<CarrierDTO>> GetByIdAsync(Guid id)
        {
            var carriers = await _carrierService.GetByIdAsync(id);
            return Ok(carriers);
        }

        public async Task<ActionResult<CarrierDTO>> GetByPhoneNumberAsync(string phoneNumber)
        {
            var carriers = await _carrierService.GetByPhoneNumberAsync(phoneNumber);
            return Ok(carriers);
        }

        public async Task<IActionResult> UpdateAsync(UpdateCarrierDTO entity)
        {
            await _carrierService.UpdateAsync(entity);
            return NoContent();
        }

        public async Task<IActionResult> UpdateIsActiveAsync(Guid id, bool isActive)
        {
            await _carrierService.UpdateIsActiveAsync(id, isActive);
            return NoContent();
        }
    }
}
