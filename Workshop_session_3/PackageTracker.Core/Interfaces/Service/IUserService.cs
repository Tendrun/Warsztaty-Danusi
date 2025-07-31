using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Service
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(Guid id);
        Task<UserDTO?> AddAsync(CreateUserDTO entity);
        Task UpdateAsync(UpdateUserDTO entity);
        Task DeleteAsync(Guid id);

        Task<UserDTO?> GetByUsernameAsync(string username);
        /// Wiele osób może mieszkać pod jednym adresem
        Task<IEnumerable<UserDTO?>> GetByAddressAsync(string address);

        Task UpdateUsernameAsync(Guid id, string username);
        Task UpdatePasswordAsync(Guid id, string password);
        Task UpdateFirstNameAsync(Guid id, string firstName);
        Task UpdateLastNameAsync(Guid id, string lastName);
    }
}
