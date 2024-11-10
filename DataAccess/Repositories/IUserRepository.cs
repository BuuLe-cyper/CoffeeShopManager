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
        public Task<User?> Login(string username, string password);
        public Task UpdateUserAsync(User user);
    }
}
