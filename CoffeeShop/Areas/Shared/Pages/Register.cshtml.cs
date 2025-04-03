using AutoMapper;
using CoffeeShop.Helper;
using CoffeeShop.Services;
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
        private readonly ApiClientService _service;
        public RegisterModel(ApiClientService service)
        {
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
            if (!ModelState.IsValid)
                return Page();
            var registrationData = new RegisterVM(UserName, Password,EmailAddress);

            var response = await _service.PostAsync<string>("User/register", registrationData);

            if (response.IsSuccess)
            {
                return RedirectToPage("./Login");
            }
            else
            {
                // Handle API errors
                ErrorMessage = response.ErrorMessage ?? "Registration failed. Please try again later.";
                return Page();
            }
        }
    }
}
