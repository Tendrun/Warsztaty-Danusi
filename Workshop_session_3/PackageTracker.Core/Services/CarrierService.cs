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

        public CarrierService(ICarrierRepository carrierRepository, Mapper mapper) { 
            _carrierRepository = carrierRepository; 
            _mapper = mapper;
        }

        public async Task<Carrier> AddAsync(CreateCarrierDTO entity)
        {
            var carrier = _mapper.Map<Carrier>(entity);

            return await _carrierRepository.AddAsync(carrier);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _carrierRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Carrier>> GetAllAsync()
        {
            return await _carrierRepository.GetAllAsync();
        }

        public async Task<Carrier?> GetByEmailAsync(string email)
        {
            return await _carrierRepository.GetByEmailAsync(email);
        }

        public async Task<Carrier?> GetByIdAsync(Guid id)
        {
            return await _carrierRepository.GetByIdAsync(id);
        }

        public async Task<Carrier?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _carrierRepository.GetByPhoneNumberAsync(phoneNumber);
        }

        public async Task UpdateAsync(UpdateCarrierDTO entity)
        {
            var carrier = _mapper.Map<Carrier>(entity);

            await _carrierRepository.UpdateAsync(carrier);
        }

        public async Task UpdateIsActiveAsync(Guid id, bool isActive)
        {
            await _carrierRepository.UpdateIsActiveAsync(id, isActive);
        }
    }
}
