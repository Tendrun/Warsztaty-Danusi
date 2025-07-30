using Microsoft.EntityFrameworkCore;
using PackageTracker.Core.Data;
using PackageTracker.Core.Entities;
using PackageTracker.Core.Interfaces.Repository;
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
    }
}
