using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces
{
    public interface IPackageRepository : IRepository<Package>
    {
        Task<IEnumerable<Package>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Package>> GetByCarrierIdAsync(int carrierId);
        Task<IEnumerable<Package>> GetByStatusAsync(string status);
        Task<Package> GetByTrackingNumberAsync(string trackingNumber);
        Task UpdateStatusAsync(int id, string status, string notes = null);
        Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(int packageId);
    }
}
