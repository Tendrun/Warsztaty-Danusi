using PackageTracker.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Repositories.Factory
{
    public interface IRepositoryFactory
    {
        IPackageRepository CreatePackageRepository();
        ICarrierRepository CreateCarrierRepository();
        IUserRepository CreateUserRepository();
    }
}
