using PackageTracker.Core.DTOs.PackageDTO;
using PackageTracker.Core.DTOs.Status;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Services
{
    public interface IPackageService
    {
        Task<IEnumerable<PackageDto>> GetAllAsync();
        Task<PackageDto?> GetByIdAsync(Guid id);
        Task<PackageDto> CreateAsync(CreatePackageDto dto);
        Task UpdateAsync(Guid id, UpdatePackageDto dto);
        Task DeleteAsync(Guid id);
        Task UpdateStatusAsync(Guid id, string status, string? notes = null);
        Task<IEnumerable<PackageStatusHistoryDTO>> GetStatusHistoryAsync(Guid packageId);
        Task<PackageDto?> GetByTrackingNumberAsync(string trackingNumber);
    }
}
