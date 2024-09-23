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
    public class UserRepository : IUserRepository
    {
        protected readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> FindAllUserAsync(string? includePropeties = null)
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> FindUserByEmail(string email)
        {
            return await _context.Users.FirstAsync(x => x.Email == email);
        }

        public async Task<User?> FindUserByID(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.UserID == id);
        }

        public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public User? Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u=>u.Username==username&&u.Password==password);
            return user;
        }

        public async Task RemoveAsync(User entity)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
