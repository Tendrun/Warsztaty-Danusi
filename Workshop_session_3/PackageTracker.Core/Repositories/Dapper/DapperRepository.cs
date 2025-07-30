using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PackageTracker.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.Dapper
{
    // DapperRepository.cs - Base Dapper repository
    public abstract class DapperRepository<T> : IRepository<T> where T : class
    {
        protected readonly string _connectionString;

        protected DapperRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T> GetByIdAsync(Guid id);
        public abstract Task<T> AddAsync(T entity);
        public abstract Task UpdateAsync(T entity);
        public abstract Task DeleteAsync(Guid id);

        // Abstract methods to be implemented by derived classes
    }
}
