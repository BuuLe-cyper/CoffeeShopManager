using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using CoffeeShopAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using CoffeeShopAPI.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CoffeeShop.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service, IMapper mapper, TokenService tkService) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;
        private readonly TokenService _tokenService = tkService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var _user = await _service.GetUser(id);
            return _user == null ? NotFound() : Ok(_mapper.Map<UsersDTO>(_user));
        }

        [HttpGet, Authorize(Roles = "Admin"), EnableQuery]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetUsers();
            return users == null || !users.Any() ? NoContent() : Ok(users.AsQueryable());
        }
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] UserVM userInfo)
        {
            await _service.AddUser(_mapper.Map<UsersDTO>(userInfo));
            return NoContent();
        }
        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserVM userVM)
        {
            var dtoUser = _mapper.Map<UsersDTO>(userVM);
            await _service.UpdateUser(dtoUser);
            return NoContent();
        }
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid userId)
        {
            await _service.DeleteUser(userId);
            return NoContent();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userDTO = await _service.Login(request.Username, request.Password);
            if (userDTO == null)
            {
                return BadRequest("Username or Password Incorrect!");
            }

            var token = _tokenService.GenerateToken(userDTO.UserName, userDTO.AccountType == 1 ? "Admin" : "User");
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await _service.GetUser(req.Username) != null)
            {
                return BadRequest($"Username {req.Username} was registered");
            }
            else if (await _service.GetUserByEmail(req.Email) != null)
            {
                return BadRequest($"Email {req.Email} was registered.");
            }

            await _service.Register(req.Username, req.Password, req.Email);
            return NoContent();
        }
    }
}
