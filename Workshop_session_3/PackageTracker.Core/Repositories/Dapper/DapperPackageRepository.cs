using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
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
                (TrackingNumber, StatusId, Weight, Length, Width, Height, RecipientAddress, SenderAddress, CarrierId, 
                UserId, ServiceType, CreatedAt, UpdatedAt)
                OUTPUT INSERTED.Id
                VALUES
                (@TrackingNumber, @StatusId, @Weight, @Length, @Width, @Height, @RecipientAddress, @SenderAddress, 
                @CarrierId, @UserId, @ServiceType, @CreatedAt, @UpdatedAt)";

            using (var connection = new SqlConnection(_connectionString))
            {
                /// Create new Package and get ID
                /// You should get ID so you dont have to make another call
                var newId = await connection.ExecuteScalarAsync<Guid>(sql, entity);
                entity.Id = newId;
                return entity;
            }
        }

        public async override Task DeleteAsync(Guid id)
        {
            string sql = @"DELETE FROM PACKAGES 
                                WHERE Id = @id";

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

        public async Task<IEnumerable<Package>> GetByCarrierIdAsync(Guid carrierId)
        {
            string sql = @"SELECT * FROM Packages WHERE CarrierId = @carrierId;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var packages = (await connection.QueryAsync<Package>(sql, new { carrierId })).ToList();
                return packages;
            }            
        }

        public async override Task<Package> GetByIdAsync(Guid id)
        {
            string sql = @"SELECT * FROM Packages
                                WHERE Id = @id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var package = await connection.QueryFirstOrDefaultAsync<Package>(sql, new { id }) 
                    ?? Package.CreateEmpty();
                return package;
            }
        }

        public async Task<IEnumerable<Package>> GetByStatusAsync(string status)
        {
            string sql = @"SELECT P.*, S.Name FROM Packages AS P
                                INNER JOIN PackageStatuses AS S ON 
                                P.StatusId = S.Id
                                WHERE S.Name = @status";

            using (var connection = new SqlConnection(_connectionString))
            { 
                var packages = (await connection.QueryAsync<Package>(sql, new { status })).ToList();
                return packages;
            }
        }

        public async Task<Package> GetByTrackingNumberAsync(string trackingNumber)
        {
            string sql = @"SELECT * FROM Packages 
                                WHERE Packages.TrackingNumber = @trackingNumber";

            using (var connection = new SqlConnection(_connectionString))
            {
                var package = await connection.QueryFirstOrDefaultAsync<Package>(sql, new { trackingNumber })
                    ?? Package.CreateEmpty();
                return package;
            }
        }

        public async Task<IEnumerable<Package>> GetByUserIdAsync(Guid userId)
        {
            string sql = @"SELECT * FROM Packages 
                                WHERE Packages.UserId = @userId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var packages = (await connection.QueryAsync<Package>(sql, new { userId })).ToList();
                return packages;
            }
        }

        public async Task<IEnumerable<PackageStatusHistory>> GetStatusHistoryAsync(Guid packageId)
        {
            string sql = @"
                            SELECT h.* FROM Packages AS P
                            INNER JOIN PackageStatusHistory AS h ON 
                            P.Id = h.PackageId
                            WHERE p.UserId = @packageId";

            using (var connection = new SqlConnection(_connectionString))
            {
                var packages = (await connection.QueryAsync<PackageStatusHistory>(sql, new { packageId })).ToList();
                return packages;
            }
        }

        public async override Task UpdateAsync(Package entity)
        {
            string sql = @"
                    Update Packages
	                SET 
	                    TrackingNumber = @TrackingNumber,
	                    StatusId = @StatusId,
	                    Weight = @Weight,
	                    Length = @Length,
	                    Width = @Width,
	                    Height = @Height,
	                    RecipientAddress = @RecipientAddress,
	                    SenderAddress = @SenderAddress,
	                    CarrierId = @CarrierId,
	                    UserId = @UserId,
	                    ServiceType = @ServiceType,
	                    CreatedAt = @CreatedAt,
	                    UpdatedAt = @UpdatedAt
		            WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task UpdateStatusAsync(Guid id, string status, string notes = null)
        { 
            using(var connection = new SqlConnection(_connectionString))
            {
                /// Get status Id
                string sqlGetStatusId = @"
                                SELECT Id 
                                FROM PackageStatuses AS ps
	                            WHERE ps.Name = @status";

                var statusId = await connection.QueryFirstOrDefaultAsync<Guid>(sqlGetStatusId, new { status });

                if (statusId == Guid.Empty)
                    return;

                /// Update package status
                string sqlUpdateStatus = @"
                            Update P
	                        SET 
	                        p.StatusId = @sqlGetStatusId
	                        FROM Packages AS P
		                    WHERE p.Id = @id";

                await connection.ExecuteAsync(sqlUpdateStatus, new { status, statusId });

                /// Update Package History
                if(!string.IsNullOrEmpty(notes))
                {
                    string sqlUpdatePackageHistory = @"
                                                INSERT INTO PackageStatusHistory (PackageId, StatusId, Timestamp, 
                                                Notes)
	                                            VALUES(@id, @getStatusId, GETDATE(), @notes)";

                    await connection.ExecuteAsync(sqlUpdatePackageHistory, new { id, statusId, notes });
                }
            }
        }

        // Implementation using SQL queries and Dapper methods
        // Use JOINs to fetch related entities
        // Use transactions for status updates
    }

    // Similar implementations for DapperCarrierRepository and DapperUserRepository
}
