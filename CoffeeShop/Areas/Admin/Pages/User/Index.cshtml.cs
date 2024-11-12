using AutoMapper;
using BussinessObjects.DTOs;
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
        public async Task<IActionResult> OnGetAsync(string? filter,int pageIndex = 1,int pageSize = 5)
        {
            IEnumerable<UsersDTO> users = [];
            if(filter!=null)
            {   
                string[] filters = filter.Split('-');
                users = await _service.GetUsersWithFilter(filters[0], bool.Parse(filters[1]), filters[2]);
            }
            else users = await _service.GetUsers();
            List<UserVM> dislayUsers = [];
            foreach (var user in users)
            {
                var vmUser = _mapper.Map<UserVM>(user);
                dislayUsers.Add(vmUser);
            }
            Users = PaginatedList<UserVM>.Create(dislayUsers, pageIndex, pageSize);
            PageSizeList = new(new[] {5,10,15,20}, selectedValue: pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync(string searchBy, string search)
        {
            IEnumerable<UsersDTO> users = await _service.SearchUsers(searchBy,search);
            List<UserVM> displayUsers = [];
            foreach (var user in users)
            {
                var _user = _mapper.Map<UserVM>(user);
                displayUsers.Add(_user);
            }
            Users = PaginatedList<UserVM>.Create(displayUsers, 1, displayUsers.Count);
            PageSizeList = new(new[] { 0 });
            return Page();
        }
    }
}
