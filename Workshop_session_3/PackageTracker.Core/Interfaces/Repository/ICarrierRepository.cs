using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.DTOs.Carrier;
using PackageTracker.Core.DTOs.CarrierService;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Repository
{
    public interface ICarrierRepository : IRepository<Carrier>
    {
        Task<Carrier> GetByEmailAsync(string email);
        Task<Carrier> GetByPhoneNumberAsync(string phoneNumber);
        Task UpdateIsActiveAsync(Guid id, bool isActive);
        Task <IEnumerable<string>> GetServicesSupportedByCarrier(Guid id);
        Task UpdateCarrierServiceInformation(Guid id, List<UpdateCarrierService> updateCarrierServices);
        new abstract Task<Carrier> AddAsync(Carrier carrier, List<CarrierService> carrierServices,
            List<SupportedServicesDto> supportedCarrierServices);
        new abstract Task UpdateAsync(Guid id, Carrier entity);
    }
}
