using PackageTracker.Core.DTOs.Auth;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string username);
        /// Wiele osób może mieszkać pod jednym adresem
        Task<IEnumerable<User>> GetByAddressAsync(string address);

        Task UpdateUsernameAsync(Guid id, string username);
        Task UpdatePasswordAsync(Guid id, string password);
        Task UpdateFirstNameAsync(Guid id, string firstName);
        Task UpdateLastNameAsync(Guid id, string lastName);

        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        Task<TokenJwtResponseDto> Login(LoginDTO loginDTO);
    }
}
