using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Areas.Admin.Pages.User
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public SelectList PageSizeList { get; set; } = new(new[] { 5, 10, 15, 20 }, selectedValue: 5);
        public IndexModel(IUserService userService, IMapper mapper)
        {
            _service = userService;
            _mapper = mapper;
        }
        public PaginatedList<UserVM> Users { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int pageIndex = 1,int pageSize = 5)
        {
            var dtoUsers = await _service.GetUsers();
            List<UserVM> users = [];
            foreach (var user in dtoUsers)
            {
                var vmUser = _mapper.Map<UserVM>(user);
                users.Add(vmUser);
            }
            Users = PaginatedList<UserVM>.Create(users, pageIndex, pageSize);
            PageSizeList = new(new[] {5,10,15,20}, selectedValue: pageSize);
            return Page();
        }
    }
}
