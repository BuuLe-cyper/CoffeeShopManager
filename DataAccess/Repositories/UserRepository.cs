using DataAccess.DataContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Username==username&&u.Password==password);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<Guid> GetGuestUserIdAsync()
        {
            var guestUser = await _context.Users
                .Where(u => u.AccountType == 0)
                .Select(u => u.UserID)
                .FirstOrDefaultAsync();

            return guestUser != Guid.Empty ? guestUser : throw new Exception("Guest user not found.");
        }
    }
}
