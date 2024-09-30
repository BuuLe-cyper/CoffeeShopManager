using BussinessObjects.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Services
{
    public interface IUserService
    {
        public Task AddUser(UsersDTO user);
        public Task UpdateUser(UsersDTO user);
        public Task DeleteUser(Guid userID);
        public Task<UsersDTO> GetUser(string username);
        public Task<UsersDTO> GetUser(Guid id);
        public Task<UsersDTO> Login(string username, string password);
        public Task<IEnumerable<UsersDTO>> GetUsers(string? includeProperty = null);
    }
}
