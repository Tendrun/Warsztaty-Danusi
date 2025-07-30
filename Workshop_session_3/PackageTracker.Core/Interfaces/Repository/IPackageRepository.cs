using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Repository
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<IEnumerable<Package>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Package>> GetByCarrierIdAsync(Guid carrierId);
        Task<IEnumerable<Package>> GetByStatusAsync(string status);
        Task<Package> GetByTrackingNumberAsync(string trackingNumber);
        Task UpdateStatusAsync(Guid id, string status, string notes = null);
        Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId);
    }
}
