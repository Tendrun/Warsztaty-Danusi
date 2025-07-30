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
using static PackageTracker.Core.Utils.PasswordHashGenerator;


namespace PackageTracker.Core.Repositories.Dapper
{
    public class DapperUserRepository : DapperRepository<User>, IUserRepository
    {
        public DapperUserRepository(IConfiguration configuration) : base(configuration) { }

        public async override Task<User> AddAsync(User entity)
        {
            string sql = @"
                INSERT INTO USERS (Username, PasswordHash, FirstName, LastName, Address, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Address, @CreatedAt)";

            using (var connection = new SqlConnection(_connectionString))
            {
                /// Create new User and get ID
                /// You should get ID so you dont have to make another call
                var newId = await connection.ExecuteScalarAsync<Guid>(sql, entity);
                entity.Id = newId;
                return entity;
            }
        }

        public async override Task DeleteAsync(Guid id)
        {
            string sql = @"DELETE FROM USERS 
                                WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { id });
                return;
            }
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            string sql = @"SELECT * FROM USERS";

            using (var connection = new SqlConnection(_connectionString))
            {
                var users = (await connection.QueryAsync<User>(sql)).ToList();
                return users;
            }
        }

        public async Task<IEnumerable<User>> GetByAddressAsync(string address)
        {
            /// Latin1_General_CI_AI needs to be used 
            /// because of polish special characters
            string sql = @"
                            SELECT * FROM USERS
                            WHERE Address COLLATE Latin1_General_CI_AI = @address;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var users = (await connection.QueryAsync<User>(sql, new { address })).ToList();
                return users;
            }
        }

        public async override Task<User> GetByIdAsync(Guid id)
        {
            string sql = @"
                            SELECT * FROM USERS
                                WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { id })
                    ?? User.CreateEmpty();

                return user;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            string sql = @"
                            SELECT * FROM USERS
                            WHERE Username COLLATE Latin1_General_CI_AI = @username;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { username })
                    ?? User.CreateEmpty();

                return user;
            }
        }

        public async override Task UpdateAsync(User entity)
        {
            string sql = @"
                    Update Users
	                SET 
	                    Username = @Username,
	                    PasswordHash = @PasswordHash,
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Address = @Address,
		            WHERE Id = @Id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, entity);
            }
        }

        public async Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            string sql = @"
                    Update Users
	                SET 
                        FirstName = @firstName
		            WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, new { id, firstName });
            }
        }

        public async Task UpdateLastNameAsync(Guid id, string lastName)
        {
            string sql = @"
                    Update Users
	                SET 
                        LastName = @lastName
		            WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, new { id, lastName });
            }
        }

        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            var passwordHash = HashPassword(password);

            string sql = @"
                    Update Users
	                SET 
                        PasswordHash = @passwordHash
		            WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, new { id, passwordHash });
            }
        }

        public async Task UpdateUsernameAsync(Guid id, string username)
        {
            string sql = @"
                    Update Users
	                SET 
                        Username = @username
		            WHERE Id = @id";

            using (var connection = new SqlConnection(_connectionString))
            {
                var entityUpdated = await connection.ExecuteAsync(sql, new { id, username });
            }
        }
    }
}
