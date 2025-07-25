using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        // Wiele osób może mieszkać pod jednym adresem
        Task<IEnumerable<User>> GetByAddressAsync(string address);

        Task UpdateUsernameAsync(int id, string username);
        Task UpdatePasswordAsync(int id, string password);
        Task UpdateFirstNameAsync(int id, string firstName);
        Task UpdateLastNameAsync(int id, string lastName);
    }
}
