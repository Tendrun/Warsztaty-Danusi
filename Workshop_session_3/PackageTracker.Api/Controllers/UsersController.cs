using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Interfaces.Service;
using System.Net;
using static Dapper.SqlMapper;

namespace PackageTracker.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
        IUserService userService,
        ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            return await _userService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<UserDTO?> GetByIdAsync(Guid id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpPost("{entity}")]
        public async Task<UserDTO?> AddAsync(CreateUserDTO entity)
        {
            return await _userService.AddAsync(entity);
        }

        [HttpPut("{id}")]
        public async Task UpdateAsync(Guid id, UpdateUserDTO entity)
        {
            await _userService.UpdateAsync(id, entity);
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
        }

        [HttpGet("Username/{username}")]
        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            return await _userService.GetByUsernameAsync(username);
        }

        [HttpGet("Address/{address}")]
        /// Wiele osób może mieszkać pod jednym adresem
        public async Task<IEnumerable<UserDTO?>> GetByAddressAsync(string address)
        {
            return await _userService.GetByAddressAsync(address);
        }

        [HttpPatch("{id}")]
        public async Task UpdateUsernameAsync(Guid id, string username)
        {
            await _userService.UpdateUsernameAsync(id, username);
        }

        [HttpPatch("Password/{id}")]
        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            await _userService.UpdatePasswordAsync(id, password);
        }

        [HttpPatch("firstName/{id}")]
        public async Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            await _userService.UpdateFirstNameAsync(id, firstName);
        }

        [HttpPatch("lastName/{id}")]
        public async Task UpdateLastNameAsync(Guid id, string lastName)
        {
            await _userService.UpdateLastNameAsync(id, lastName);
        }
    }
}
