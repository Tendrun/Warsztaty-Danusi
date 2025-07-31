using PackageTracker.Core.DTOs.Carrier;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Service
{
    public interface ICarrierService
    {
        Task<CarrierDTO> AddAsync(CreateCarrierDTO entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<CarrierDTO>> GetAllAsync();
        Task<CarrierDTO?> GetByEmailAsync(string email);
        Task<CarrierDTO?> GetByIdAsync(Guid id);
        Task<CarrierDTO?> GetByPhoneNumberAsync(string phoneNumber);
        Task UpdateAsync(UpdateCarrierDTO entity);
        Task UpdateIsActiveAsync(Guid id, bool isActive);
    }
}
