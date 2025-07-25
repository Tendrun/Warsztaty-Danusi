using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces
{
    public interface ICarrierRepository : IRepository<Carrier>
    {
        Task<IEnumerable<Carrier>> GetByCarrierIdAsync(Guid CarrierId);
    }
}
