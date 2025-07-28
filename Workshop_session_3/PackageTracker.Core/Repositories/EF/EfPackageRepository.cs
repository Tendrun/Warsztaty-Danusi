using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.EF
{
    // EfPackageRepository.cs - Package-specific EF repository
    public class EfPackageRepository : EfRepository<Package>, IPackageRepository
    {
        public EfPackageRepository(ApplicationDbContext context) : base(context) { }


        // Implementation of IPackageRepository methods

        public async Task<IEnumerable<Package>> GetByCarrierIdAsync(int carrierId)
        {
            return await _dbSet
                .Where(p => p.CarrierId == carrierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(p => p.Status)
                .Where(p => p.Status.Name == status)
                .ToListAsync();
        }

        public async Task<Package> GetByTrackingNumberAsync(string trackingNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.TrackingNumber == trackingNumber)
                ?? Package.CreateEmpty();
        }

        public async Task<IEnumerable<Package>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(int packageId)
        {
            return await _context.PackageStatusHistories
                .Where(p => p.PackageId == packageId)
                .OrderByDescending(h => h.Timestamp)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, string status, string notes = null)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            var package = await _dbSet
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id);

            if(package == null) 
                return;

            var newStatus = await _context
                .PackageStatuses
                .FirstOrDefaultAsync(s => s.Name == status);

            if (newStatus == null)
                return;

            package.StatusId = newStatus.Id;
            package.UpdatedAt = DateTime.Now;

            var history = new PackageStatusHistory
            {
                PackageId = package.Id,
                Package = package,
                StatusId = newStatus.Id,
                Status = newStatus,
                Timestamp = DateTime.Now,
                Notes = notes
            };

            _context.PackageStatusHistories.Add(history);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        // Include related entities with Include()
        // Use transactions for status updates
    }

    // Similar implementations for EfCarrierRepository and EfUserRepository
}
