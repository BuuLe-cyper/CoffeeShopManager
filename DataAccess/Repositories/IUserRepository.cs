using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        public User? Login(string username, string password);
        public Task<List<User>> FindAllUserAsync(string? includePropeties = null);
        public Task<User?> FindUserByID(Guid id);
        public Task<User?> FindUserByEmail(string email);
    }
}
