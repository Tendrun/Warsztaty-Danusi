using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PackageTracker.Core.Data;
using PackageTracker.Core.Interfaces;
using PackageTracker.Core.Repositories.Dapper;
using PackageTracker.Core.Repositories.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.Factory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ApplicationDbContext _dbContext;
        private readonly string _dataAccessStrategy;

        public RepositoryFactory(
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _dbContext = dbContext;
            _dataAccessStrategy = configuration["DataAccess:Strategy"] ?? "EntityFramework";
        }

        public ICarrierRepository CreateCarrierRepository()
        {
            throw new NotImplementedException();
        }

        public IPackageRepository CreatePackageRepository()
        {
            return _dataAccessStrategy.ToLower() switch
            {
                "dapper" => new DapperPackageRepository(_configuration),
                _ => new EfPackageRepository(_dbContext)
            };
        }

        public IUserRepository CreateUserRepository()
        {
            throw new NotImplementedException();
        }

        // Similar methods for CreateCarrierRepository and CreateUserRepository
    }
}
