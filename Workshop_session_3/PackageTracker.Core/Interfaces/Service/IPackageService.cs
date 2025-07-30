using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.DTOs.PackageDTO;

namespace PackageTracker.Core.Interfaces.Services
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllAsync();
        Task<Package?> GetByIdAsync(Guid id);
        Task<Package> CreateAsync(CreatePackageDto dto);
        Task UpdateAsync(Guid id, UpdatePackageDto dto);
        Task DeleteAsync(Guid id);
        Task UpdateStatusAsync(Guid id, string status, string? notes = null);
        Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId);
        Task<Package?> GetByTrackingNumberAsync(string trackingNumber);
    }
}
