using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace PackageTracker.Core.Repositories.Dapper
{
    // DapperPackageRepository.cs - Package-specific Dapper repository
    public class DapperPackageRepository : DapperRepository<Package>, IPackageRepository
    {
        public DapperPackageRepository(IConfiguration configuration) : base(configuration) { }

        public override async Task<Package> AddAsync(Package entity)
        {
            string sql = @"
                INSERT INTO PACKAGES 
                (TrackingNumber, StatusId, Weight, Length, Width, Height, RecipientAddress, SenderAddress, CarrierId, UserId,
                ServiceType, CreatedAt, UpdatedAt)
                VALUES
                (@TrackingNumber, @StatusId, @Weight, @Length, @Width, @Height, @RecipientAddress, @SenderAddress, @CarrierId, 
                @UserId, @ServiceType, @CreatedAt, @UpdatedAt)";

            using (var connection = new SqlConnection(_connectionString))
            {
                var newId = await connection.ExecuteScalarAsync<int>(sql);
                entity.Id = newId;
                return entity;
            }
        }

        public async override Task DeleteAsync(int id)
        {
            string sql = @"
                DELETE FROM PACKAGES 
                WHERE 
                Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { id });
                return;
            }
        }

        public override async Task<IEnumerable<Package>> GetAllAsync()
        {
            string sql = "SELECT * FROM PACKAGES";

            using(var connection = new SqlConnection(_connectionString))
            {
                var packages = (await connection.QueryAsync<Package>(sql)).ToList();
                return packages;
            }
        }

        public Task<IEnumerable<Package>> GetByCarrierIdAsync(int carrierId)
        {
            /*
            string sql = @"SELECT * FROM PACKAGES
                WHERE 
                Id = @id
                Inner Join Carrier
                On ";
            */
        }

        public override Task<Package> GetByIdAsync(int id)
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

        public Task<IEnumerable<Package>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(int packageId)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Package entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateStatusAsync(int id, string status, string notes = null)
        {
            throw new NotImplementedException();
        }

        // Implementation using SQL queries and Dapper methods
        // Use JOINs to fetch related entities
        // Use transactions for status updates
    }

    // Similar implementations for DapperCarrierRepository and DapperUserRepository
}
