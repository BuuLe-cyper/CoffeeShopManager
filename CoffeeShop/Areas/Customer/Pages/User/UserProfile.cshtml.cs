using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Areas.Customer.Pages.User
{
    [Authorize(Roles = "User")]
    public class UserProfileModel(IUserService service, IMapper mapper) : PageModel
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;
        [BindProperty]
        public new UserVM User { get; set; } = default!;
        public string ErrorMessage { get; set; } = string.Empty;
        public void OnGet()
        {
            string? userJson = HttpContext.Session.GetString("User");
            if (userJson != null)
            {
                var userObject = JsonDeserializeHelper.DeserializeObject<UserVM>(userJson);
                if (userObject != null)
                {
                    User = userObject;
                }
                else
                {
                    ErrorMessage = "Cast user failed";
                }
            }
            else
            {
                ErrorMessage = "Cant get user from session";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateUser(_mapper.Map<UsersDTO>(User));
            }
            else ErrorMessage = "Updated Fail";
            return Page();
        }
    }
}
