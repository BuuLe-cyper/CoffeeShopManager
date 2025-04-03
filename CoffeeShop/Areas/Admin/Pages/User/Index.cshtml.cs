using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.Services;
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
        private const string ENDPOINT = "User";
        private readonly ApiClientService _service;
        private readonly IMapper _mapper;
        public SelectList PageSizeList { get; set; } = new(new[] { 5, 10, 15, 20 }, selectedValue: 5);
        public IndexModel(ApiClientService clientSv, IMapper mapper)
        {
            _service = clientSv;
            _mapper = mapper;
        }
        public PaginatedList<UserVM> Users { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string? filter,int pageIndex = 1,int pageSize = 5)
        {

            var _token = HttpContext.Session.GetString("AuthToken");
            if(_token == null)
            {
                return Redirect("/Shared/Login");
            }
            _service.SetBearerToken(_token);

            IEnumerable<UsersDTO> users = [];
            if(filter!=null)
            {   
                string[] filters = filter.Split('-');
                var sortBy = $"?$orderby= {filters[0]} {filters[1]}";
                string displayRole = "";
                switch(filters[2])
                {
                    case "all":
                        break;
                    case "admin":
                        displayRole = "& $filter = AccountType eq 1";
                        break;
                    case "user":
                        displayRole = "& $filter = AccountType eq 0";
                        break;
                }
                string trueEndpoint = ENDPOINT + sortBy + displayRole;
                users = (await _service.GetAsync<IEnumerable<UsersDTO>>(trueEndpoint)).Data;
            }
            else users = (await _service.GetAsync<IEnumerable<UsersDTO>>(ENDPOINT)).Data;

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
            var _token = HttpContext.Session.GetString("AuthToken");
            if (_token == null)
            {
                return Redirect("/Shared/Login");
            }
            _service.SetBearerToken(_token);
            var endPoint = ENDPOINT + $"? $filter = contains({searchBy},'{search}')";
            IEnumerable<UsersDTO> users = (await _service.GetAsync< IEnumerable<UsersDTO>>(endPoint)).Data;
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
