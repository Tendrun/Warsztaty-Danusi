using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackageTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.Interfaces;

namespace PackageTracker.Core.Repositories.EF
{
    public class EfCarrierRepository : EfRepository<Carrier>, ICarrierRepository
    {
        public EfCarrierRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Carrier> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email)
                ?? Carrier.createEmpty();
        }

        public async Task<Carrier> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber)
                ?? Carrier.createEmpty();
        }

        public async Task UpdateIsActiveAsync(int id, bool isActive)
        {
            var carrier = await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrier == null)
                return;
            
            /// Update isActive
            carrier.IsActive = isActive;

            await _context.SaveChangesAsync();
        }
    }
}
