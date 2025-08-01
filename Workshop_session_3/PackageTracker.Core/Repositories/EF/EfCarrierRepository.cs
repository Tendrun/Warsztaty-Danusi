using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PackageTracker.Core.Data;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.EF
{
    public class EfCarrierRepository : EfRepository<Carrier>, ICarrierRepository
    {
        public EfCarrierRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Carrier> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email)
                ?? Carrier.CreateEmpty();
        }

        public async Task<Carrier> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber)
                ?? Carrier.CreateEmpty();
        }

        public async Task UpdateIsActiveAsync(Guid id, bool isActive)
        {
            var carrier = await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id);

            if (carrier == null)
                return;
            
            /// Update isActive
            carrier.IsActive = isActive;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetServicesSupportedByCarrier(Guid id)
        {
            var supportedServices = await _context
                .CarrierSupportedServices
                .Where(css => css.CarrierId == id)
                .ToListAsync();

            var supportedServicesIds = supportedServices.Select(u => u.ServiceId).ToList();

            var serviceTypes = await _context
                .carrierServices
                .Where(cs => supportedServicesIds.Contains(cs.Id))
                .ToListAsync();

            var serviceTypesFormated = serviceTypes.Select(u => u.Name).ToList();

            return serviceTypesFormated;
        }
    }
}
