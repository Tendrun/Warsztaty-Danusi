using AutoMapper;
using PackageTracker.Core.DTOs.Auth;
using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.Interfaces.Service;
using PackageTracker.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using static PackageTracker.Core.Utils.PasswordHashGenerator;

namespace PackageTracker.Core.Services
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;
        Mapper mapper;

        public UserService(IUserRepository userRepository, Mapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserDTO?> AddAsync(CreateUserDTO entity)
        {
            var user = mapper.Map<User>(entity);

            user.PasswordHash = HashPassword(entity.Password);

            var userAdded = await userRepository.AddAsync(user);

            var userDTO = mapper.Map<UserDTO>(userAdded);

            return userDTO;
        }

        public async Task DeleteAsync(Guid id)
        {
            await userRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await userRepository.GetAllAsync();
            var usersDTO = mapper.Map<List<UserDTO>>(users);

            return usersDTO;
        }

        public async Task<IEnumerable<UserDTO?>> GetByAddressAsync(string address)
        {
            var users = await userRepository.GetByAddressAsync(address);
            var userDTOs = mapper.Map<List<UserDTO>>(users);

            return userDTOs;
        }

        public async Task<UserDTO?> GetByIdAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            var userDTO = mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<UserDTO?> GetByUsernameAsync(string username)
        {
            var user = await userRepository.GetByUsernameAsync(username);
            var userDTO = mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<TokenJwtResponseDto> Login(LoginDTO loginDTO)
        {
            return await userRepository.Login(loginDTO);
        }

        public async Task UpdateAsync(Guid id, UpdateUserDTO entity)
        {
            var user = mapper.Map<User>(entity);
            await userRepository.UpdateAsync(id, user);
        }

        public async Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            await userRepository.UpdateFirstNameAsync(id, firstName);
        }

        public async Task UpdateLastNameAsync(Guid id, string lastName)
        {
            await userRepository.UpdateFirstNameAsync(id, lastName);
        }

        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            string hashedpassword = HashPassword(password);

            await userRepository.UpdatePasswordAsync(id, hashedpassword);
        }

        public async Task UpdateUsernameAsync(Guid id, string username)
        {
            await userRepository.UpdateUsernameAsync(id, username);
        }
    }
}
