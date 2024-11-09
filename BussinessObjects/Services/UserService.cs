using AutoMapper;
using BussinessObjects.DTOs;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BussinessObjects.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _rep;
        private readonly IMapper _mapper;

        public UserService(IUserRepository rep, IMapper mapper)
        {
            _mapper = mapper;
            _rep = rep;
        }

        public async Task AddUser(UsersDTO user)
        {
            ArgumentNullException.ThrowIfNull(user);
            try
            {
                var _user = await _rep.GetAsync(u => u.UserID == user.UserID);
                if (_user != null)
                    throw new Exception("User already exist!");
                var addedUser = _mapper.Map<User>(user);
                await _rep.CreateAsync(addedUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteUser(Guid userID)
        {
            try
            {
                var deletedUser = await _rep.GetAsync(u => u.UserID == userID);
                if (deletedUser == null)
                    throw new Exception("User doesnt exist!");
                deletedUser.IsDeleted = true;
                deletedUser.IsActive = false;
                await _rep.UpdateAsync(deletedUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UsersDTO> GetUser(string username)
        {

            var user = await _rep.GetAsync(u => u.Username == username);
            return _mapper.Map<UsersDTO>(user);
        }

        public async Task<UsersDTO> GetUser(Guid id)
        {
            var user = await _rep.GetAsync(u => u.UserID == id);
            return _mapper.Map<UsersDTO>(user);
        }

        public async Task<UsersDTO> GetUserByEmail(string email)
            => _mapper.Map<UsersDTO>(await _rep.GetAsync(u => u.Email == email));

        public async Task<IEnumerable<UsersDTO>> GetUsers(string? includeProperty = null)
        {
            var users = await _rep.GetAllAsync(u => u.IsDeleted == false, includeProperty);
            var usersList = new List<UsersDTO>();
            foreach (var user in users)
            {
                var userDTO = _mapper.Map<UsersDTO>(user) as UsersDTO;
                usersList.Add(userDTO);
            }
            return usersList;
        }

        public async Task<UsersDTO> Login(string username, string password)
        {
            var user = await _rep.Login(username, password);
            return _mapper.Map<UsersDTO>(user);
        }

        public async Task Register(string username, string password, string email)
        {
            var existedUser = await _rep.GetAsync(u => u.Email == email);
            var dtoUser = new UsersDTO
            {
                UserName = username,
                Password = password,
                Email = email,
                FullName = string.Empty,
                PhoneNumber = string.Empty,
                AccountType = 0,
            };
            var registerUser = _mapper.Map<User>(dtoUser);
            await _rep.CreateAsync(registerUser);
        }

        public async Task UpdateUser(UsersDTO user)
        {
            ArgumentNullException.ThrowIfNull(user);
            try
            {
                var updatedUser = _mapper.Map<User>(user) as User;
                var _user = _rep.GetAsync(u => u.UserID == updatedUser.UserID);
                if (_user == null)
                    throw new Exception(message: "User doesnt exist!");
                updatedUser.ModifyDate = DateTime.Now;
                await _rep.UpdateUserAsync(updatedUser);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
