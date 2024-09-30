using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Admin.Pages.User
{
    [Authorize(Roles="Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public IndexModel(IUserService userService, IMapper mapper)
        {
            _service = userService;
            _mapper = mapper;
        }

        public List<UserVM> Users { get; set; } = [];

        public async Task OnGetAsync()
        {
            var dtoUsers = await _service.GetUsers();
            foreach (var user in dtoUsers)
            {
                var vmUser = _mapper.Map<UserVM>(user);
                Users.Add(vmUser);
            }
        }
    }
}
