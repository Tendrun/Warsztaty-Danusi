using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.Interfaces.Services;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.DTOs.PackageDTO;

namespace PackageTracker.Core.Services
{
    public class PackageService : IPackageService
    {
        IPackageRepository packageRepository;


        public PackageService(IPackageRepository packageRepository) { this.packageRepository = packageRepository; }

        public async Task<Package> CreateAsync(CreatePackageDto dto)
        {
            var package = convertCreatePackageDto(dto);

            return await packageRepository.AddAsync(package);
        }

        public async Task DeleteAsync(Guid id)
        {
            await packageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            return await packageRepository.GetAllAsync();
        }

        public async Task<Package?> GetByIdAsync(Guid id)
        {
            return await packageRepository.GetByIdAsync(id);
        }

        public async Task<Package?> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await packageRepository.GetByTrackingNumberAsync(trackingNumber);
        }

        public async Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId)
        {
            return await packageRepository.GetStatusHistoryAsync(packageId);
        }

        public async Task UpdateAsync(Guid id, UpdatePackageDto dto)
        {
            Package package = convertUpdatePackageDTO(id, dto);

            await packageRepository.UpdateAsync(package);
        }

        public async Task UpdateStatusAsync(Guid id, string status, string? notes = null)
        {
            await packageRepository.UpdateStatusAsync(id, status, notes);
        }


        Package convertCreatePackageDto(CreatePackageDto dto)
        {
            return new Package
            {
                TrackingNumber = dto.TrackingNumber,
                StatusId = dto.StatusId,
                Weight = dto.Weight,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,
                RecipientAddress = dto.RecipientAddress,
                SenderAddress = dto.SenderAddress,
                CarrierId = dto.CarrierId,
                UserId = dto.UserId,
                ServiceTypeId = dto.ServiceTypeId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        Package convertUpdatePackageDTO(Guid idDTO, UpdatePackageDto dto)
        {
            return new Package
            {
                Id = idDTO,
                TrackingNumber = dto.TrackingNumber,
                Weight = dto.Weight,
                Length = dto.Length,
                Width = dto.Width,
                Height = dto.Height,

                RecipientAddress = dto.RecipientAddress,
                SenderAddress = dto.SenderAddress,

                CarrierId = dto.CarrierId,
                UserId = dto.UserId,
                ServiceTypeId = dto.ServiceTypeId,

                UpdatedAt = DateTime.UtcNow,
            };
        }
    }
}
