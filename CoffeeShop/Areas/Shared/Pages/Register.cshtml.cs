using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.Validations;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [Validations.PasswordStrength]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [ConfirmPasswordValidation("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ErrorMessage = string.Empty;
                var user = await _service.GetUser(UserName);
                if (user != null)
                {
                    ErrorMessage = "This username is already exist!Please try another one";
                    return Page();
                }
                user = await _service.GetUserByEmail(EmailAddress);
                if (user != null)
                {
                    ErrorMessage = "This email is already exist!Please try another one";
                    return Page();
                }
                var userVM = new UserVM
                {
                    Username = UserName,
                    Password = Password,
                    Email = EmailAddress
                };
                var userJson = JsonDeserializeHelper.SerializeObject(userVM);
                TempData["RegisterUser"] = userJson;
                return RedirectToPage("./ConfirmEmail", new{ email= userVM.Email,action="register"});
            }
            else
                return Page();
        }
    }
}
