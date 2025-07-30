using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
    public class DapperCarrierRepository : DapperRepository<Carrier>, ICarrierRepository
    {
        public DapperCarrierRepository(IConfiguration configuration) : base(configuration) { }

        public async override Task<Carrier> AddAsync(Carrier entity)
        {
            string sql = @"
                INSERT INTO Carriers 
                (Name, Email, PhoneNumber, IsActive)
                OUTPUT INSERTED.Id
                VALUES
                (@Name, @Email, @PhoneNumber, @IsActive)";

            using (var connection = new SqlConnection(_connectionString))
            {
                /// Create new Carrier and get ID
                /// You should get ID so you dont have to make another call
                var newId = await connection.ExecuteScalarAsync<Guid>(sql, entity);
                entity.Id = newId;
                return entity;
            }
        }

        public async override Task DeleteAsync(Guid id)
        {
            string sql = @"DELETE FROM CARRIERS 
                                WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { id });
                return;
            }
        }

        public async override Task<IEnumerable<Carrier>> GetAllAsync()
        {
            string sql = "SELECT * FROM CARRIERS";

            using (var connection = new SqlConnection(_connectionString))
            {
                var carriers = (await connection.QueryAsync<Carrier>(sql)).ToList();
                return carriers;
            }
        }

        public async Task<Carrier> GetByEmailAsync(string email)
        {
            string sql = @"SELECT * FROM CARRIERS
                            WHERE email = @email";

            using (var connection = new SqlConnection(_connectionString))
            {
                var carrier = await connection.QueryFirstOrDefaultAsync<Carrier>(sql, new { email })
                    ?? Carrier.CreateEmpty();
                return carrier;
            }
        }

        public async override Task<Carrier> GetByIdAsync(Guid id)
        {
            string sql = @"SELECT * FROM CARRIERS
                            WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var carrier = await connection.QueryFirstOrDefaultAsync<Carrier>(sql, new { id })
                    ?? Carrier.CreateEmpty();
                return carrier;
            }
        }

        public async Task<Carrier> GetByPhoneNumberAsync(string phoneNumber)
        {
            string sql = @"SELECT * FROM CARRIERS
                            WHERE PhoneNumber = @phoneNumber";

            using (var connection = new SqlConnection(_connectionString))
            {
                var carrier = await connection.QueryFirstOrDefaultAsync<Carrier>(sql, new { phoneNumber })
                    ?? Carrier.CreateEmpty();
                return carrier;
            }
        }

        public async override Task UpdateAsync(Carrier entity)
        {
            string sql = @"
                    Update Carriers
                    SET
	                    Name = @Name,
	                    Email = @Email,
	                    PhoneNumber = @PhoneNumber,
	                    IsActive = @IsActive
                    WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task UpdateIsActiveAsync(Guid id, bool isActive)
        {
            string sql = @"
                    Update Carriers 
                    SET
	                    IsActive = @isActive
                    WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, new { id, isActive });
            }
        }
    }
}
