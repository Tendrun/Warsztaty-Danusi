using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
                ?? User.createEmpty();
        }

        public async Task UpdateFirstNameAsync(int id, string firstName)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return;

            user.FirstName = firstName;
            _context.SaveChanges();
        }

        public async Task UpdateLastNameAsync(int id, string lastName)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return;

            user.LastName = lastName;
            _context.SaveChanges();
        }

        public async Task UpdatePasswordAsync(int id, string password)
        {
            string hashPassowrd = HashPassword(password);

            var user = await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
                return;

            user.PasswordHash = hashPassowrd;

            _context.SaveChanges();
        }

        public async Task UpdateUsernameAsync(int id, string username)
        {
            var Username = await _dbSet
                .FirstOrDefaultAsync(u => u.Id == id);

            if (Username == null) 
                return;

            Username.Username = username;

            _context.SaveChanges();
        }

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // Połącz sól i hash razem
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
        }

    }
}
