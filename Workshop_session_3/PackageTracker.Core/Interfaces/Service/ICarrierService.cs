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
        Task<Carrier> AddAsync(Carrier entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Carrier>> GetAllAsync();
        Task<Carrier> GetByEmailAsync(string email);
        Task<Carrier> GetByIdAsync(Guid id);
        Task<Carrier> GetByPhoneNumberAsync(string phoneNumber);
        Task UpdateAsync(Carrier entity);
        Task UpdateIsActiveAsync(Guid id, bool isActive);
    }
}
