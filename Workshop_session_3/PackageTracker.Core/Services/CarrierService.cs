using AutoMapper;
using PackageTracker.Core.DTOs.Carrier;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Services
{
    public class CarrierService : ICarrierService
    {
        ICarrierRepository _carrierRepository;
        private readonly IMapper _mapper;

        public CarrierService(ICarrierRepository carrierRepository, IMapper mapper) { 
            _carrierRepository = carrierRepository; 
            _mapper = mapper;
        }

        public async Task<CarrierDTO> AddAsync(CreateCarrierDTO entity)
        {
            var carrier = _mapper.Map<Carrier>(entity);
            var carrierDTO = await _carrierRepository.AddAsync(carrier);

            return _mapper.Map<CarrierDTO>(carrierDTO);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _carrierRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CarrierDTO>> GetAllAsync()
        {
            var carriers = await _carrierRepository.GetAllAsync();
            var carriersDTO = _mapper.Map<List<CarrierDTO>>(carriers);

            return carriersDTO;
        }

        public async Task<CarrierDTO?> GetByEmailAsync(string email)
        {
            var carrier = await _carrierRepository.GetByEmailAsync(email);
            var carrierDTO = _mapper.Map<CarrierDTO>(carrier);

            return carrierDTO;
        }

        public async Task<CarrierDTO?> GetByIdAsync(Guid id)
        {
            var carrier = await _carrierRepository.GetByIdAsync(id);
            var carrierDTO = _mapper.Map<CarrierDTO>(carrier);

            return carrierDTO;
        }

        public async Task<CarrierDTO?> GetByPhoneNumberAsync(string phoneNumber)
        {
            var carrier = await _carrierRepository.GetByPhoneNumberAsync(phoneNumber);
            var carrierDTO = _mapper.Map<CarrierDTO>(carrier);

            return carrierDTO;
        }

        public async Task UpdateAsync(Guid id, UpdateCarrierDTO entity)
        {
            var carrier = _mapper.Map<Carrier>(entity);

            await _carrierRepository.UpdateAsync(id, carrier);
        }

        public async Task UpdateIsActiveAsync(Guid id, bool isActive)
        {
            await _carrierRepository.UpdateIsActiveAsync(id, isActive);
        }

        public async Task<IEnumerable<string>> GetServicesSupportedByCarrier(Guid id)
        {
            return await _carrierRepository.GetServicesSupportedByCarrier(id);
        }
    }
}
