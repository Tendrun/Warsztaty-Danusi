using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.DTOs.Auth;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
using PackageTracker.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static PackageTracker.Core.Utils.PasswordHashGenerator;

namespace PackageTracker.Core.Repositories.EF
{
    public class EfUserRepository : EfRepository<User>, IUserRepository
    {
        public EfUserRepository(ApplicationDbContext context) : base(context) { }
        public async Task<IEnumerable<User>> GetByAddressAsync(string address)
        {
            return await _dbSet
                .Where(u => u.Address == address)
                .ToListAsync();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username)
                ?? User.CreateEmpty();
        }

        public async Task UpdateFirstNameAsync(Guid id, string firstName)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return;

            user.FirstName = firstName;
            _context.SaveChanges();
        }

        public async Task UpdateLastNameAsync(Guid id, string lastName)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return;

            user.LastName = lastName;
            _context.SaveChanges();
        }

        public async Task UpdatePasswordAsync(Guid id, string password)
        {
            string hashPassowrd = HashPassword(password);

            var user = await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
                return;

            user.PasswordHash = hashPassowrd;

            _context.SaveChanges();
        }

        public async Task UpdateUsernameAsync(Guid id, string username)
        {
            var Username = await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Username == null) 
                return;

            Username.Username = username;

            _context.SaveChanges();
        }

        public async Task<TokenJwtResponseDto> Login(LoginDTO loginDTO)
        {
            var user = await _dbSet
                .FirstOrDefaultAsync(u => u.Email == loginDTO.Email);


            if (user == null)
                return new TokenJwtResponseDto
                {
                    TokenJWT = ""
                };

            bool isPasswordValid = VerifyPassword(loginDTO.Password, user.PasswordHash);

            if (!isPasswordValid)
                return new TokenJwtResponseDto
                {
                    TokenJWT = ""
                };


            string token = TokenJwtGenerator.GenerateToken(user.Username, "User");

            return new TokenJwtResponseDto
            {
                TokenJWT = token
            };
        }

        public override async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.Include(u => u.Packages).FirstOrDefaultAsync(u => u.Id == id);


            if (user != null)
            {
                // Usuń powiązane paczki
                _context.packages.RemoveRange(user.Packages);

                // Usuń użytkownika
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();
            }
        }
    }
}
