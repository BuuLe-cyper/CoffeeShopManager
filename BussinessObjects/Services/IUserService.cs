using BussinessObjects.DTOs;

namespace BussinessObjects.Services
{
    public interface IUserService
    {
        public Task AddUser(UsersDTO user);
        public Task UpdateUser(UsersDTO user);
        public Task DeleteUser(Guid userID);
        public Task<UsersDTO> GetUser(string username);
        public Task<UsersDTO> GetUserByEmail(string email);
        public Task<UsersDTO> GetUser(Guid id);
        public Task<UsersDTO> Login(string username, string password);
        public Task<IEnumerable<UsersDTO>> GetUsers(string? includeProperty = null);
        public Task Register(string username, string password,string email);
        public Task<IEnumerable<UsersDTO>> GetUsersWithFilter(string sortBy, bool direction, string accountType);
        public Task<IEnumerable<UsersDTO>> SearchUsers(string searchBy, string search);
        Task<Guid> GetGuestUserIdAsync();
    }
}
