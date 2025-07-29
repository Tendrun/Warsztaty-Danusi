using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.Dapper
{
    public class DapperCarrierRepository : DapperRepository<Package>, IPackageRepository
    {
        public DapperCarrierRepository(IConfiguration configuration) : base(configuration) { }

        public override Task<Package> AddAsync(Package entity)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Package>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetByCarrierIdAsync(Guid carrierId)
        {
            throw new NotImplementedException();
        }

        public override Task<Package> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetByStatusAsync(string status)
        {
            throw new NotImplementedException();
        }

        public Task<Package> GetByTrackingNumberAsync(string trackingNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Package>> GetByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Package entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(Guid id, string status, string notes = null)
        {
            throw new NotImplementedException();
        }
    }
}
