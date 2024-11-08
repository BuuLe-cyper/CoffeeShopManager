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
        public new UserVM? User { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public IActionResult OnGet()
        {
            string? userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
                return RedirectToPage("/Login");

            var userObject = JsonDeserializeHelper.DeserializeObject<UserVM>(userJson);
            if (userObject != null)
            {
                User = userObject;
            }
            else
            {
                ErrorMessage = "There are some unexpected error has occur. Please try access this page later!";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dtoUser = _mapper.Map<UsersDTO>(User);
                    await _service.UpdateUser(dtoUser);

                    //update new user info into session
                    string userJson = JsonDeserializeHelper.SerializeObject(User);
                    HttpContext.Session.SetString("User", userJson);
                }
                catch (Exception)
                {
                    ErrorMessage = "Updated Fail";
                }

            }
            else ErrorMessage = "Updated Fail";
            return Page();
        }
    }
}
