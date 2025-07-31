using Microsoft.AspNetCore.Mvc;
using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Interfaces.Service;

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

        public async Task UpdateAsync(UpdateUserDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();

        }

        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            throw new NotImplementedException();

        }

        /// Wiele osób może mieszkać pod jednym adresem
        public async Task<IEnumerable<UserDTO?>> GetByAddressAsync(string address)
        {
            throw new NotImplementedException();

        }

        public async Task UpdateUsernameAsync(Guid id, string username)
        {
            throw new NotImplementedException();

        }

        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            throw new NotImplementedException();

        }

        public async Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            throw new NotImplementedException();

        }

        public async Task UpdateLastNameAsync(Guid id, string lastName)
        {
            throw new NotImplementedException();

        }
    }
}
