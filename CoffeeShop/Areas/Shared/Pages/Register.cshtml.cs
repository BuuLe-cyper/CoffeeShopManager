using AutoMapper;
using BussinessObjects.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Areas.Shared.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;
        public RegisterModel(IMapper mapper, IUserService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [BindProperty]
        [Required]
        public string UserName { get; set; } = string.Empty;
        [BindProperty]
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        public string ErrorMessage { get; set; }= string.Empty;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _service.GetUser(UserName);
                if(user != null)
                {
                    ErrorMessage = "This username already exist!Please try another one";
                    return Page();
                }
                await _service.Register(UserName,Password,EmailAddress);
                user = await _service.GetUser(UserName);//get again to check if the user is created;
                if (user != null) return RedirectToPage("./Login");
                else
                {
                    ErrorMessage = "Create users failed. Please try again later!";
                    return Page();
                }
            }
            else
                return Page();
        }
    }
}
