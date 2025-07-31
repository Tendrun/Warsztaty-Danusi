using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.Interfaces.Services;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.DTOs.PackageDTO;
using PackageTracker.Core.DTOs.Status;
using AutoMapper;

namespace PackageTracker.Core.Services
{
    public class PackageService : IPackageService
    {
        IPackageRepository packageRepository;
        IMapper mapper;

        public PackageService(IPackageRepository packageRepository, IMapper mapper) 
        { 
            this.packageRepository = packageRepository; 
            this.mapper = mapper;
        }

        public async Task<PackageDto> CreateAsync(CreatePackageDto dto)
        {
            var package = convertCreatePackageDto(dto);
            var newPackage = await packageRepository.AddAsync(package);

            var packageDTO = mapper
                .Map<PackageDto>(newPackage);

            return packageDTO;
        }

        public async Task DeleteAsync(Guid id)
        {
            await packageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PackageDto>> GetAllAsync()
        {
            var packages = await packageRepository.GetAllAsync();
            var packagesDTO = mapper.Map<List<PackageDto>>(packages);

            return packagesDTO;
        }

        public async Task<PackageDto?> GetByIdAsync(Guid id)
        {
            var package = await packageRepository.GetByIdAsync(id);
            var packageDTO = mapper.Map<PackageDto>(package);

            return packageDTO;
        }

        public async Task<PackageDto?> GetByTrackingNumberAsync(string trackingNumber)
        {
            var package = await packageRepository.GetByTrackingNumberAsync(trackingNumber);
            var packageDTO = mapper.Map<PackageDto>(package);

            return packageDTO;
        }

        public async Task<IEnumerable<PackageStatusHistoryDTO>> GetStatusHistoryAsync(Guid packageId)
        {
            var packages = await packageRepository.GetStatusHistoryAsync(packageId);
            var packagesDTO = mapper.Map<List<PackageStatusHistoryDTO>>(packages);

            return packagesDTO;
        }

        public async Task UpdateAsync(Guid id, UpdatePackageDto dto)
        {
            Package package = convertUpdatePackageDTO(id, dto);

            await packageRepository.UpdateAsync(id, package);
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
