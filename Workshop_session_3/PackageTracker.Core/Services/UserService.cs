using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Services
{
    public class UserService : IUserService
    {
        public Task<UserDTO?> AddAsync(CreateUserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDTO?>> GetByAddressAsync(string address)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UpdateUserDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLastNameAsync(Guid id, string lastName)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePasswordAsync(Guid id, string password)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUsernameAsync(Guid id, string username)
        {
            throw new NotImplementedException();
        }
    }
}
