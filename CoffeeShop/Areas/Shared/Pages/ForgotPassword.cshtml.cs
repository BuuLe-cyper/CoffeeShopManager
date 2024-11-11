using BussinessObjects.Services;
using CoffeeShop.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Areas.Shared.Pages
{
    [AllowAnonymous]
    public class ForgotPasswordModel(IUserService service) : PageModel
    {
        private readonly IUserService _service = service;
        public string? ErrorMessage { get; set; }
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = string.Empty;

        [BindProperty]
        [DataType(DataType.Password)]
        [Validations.PasswordStrength]
        [Display(Name = "New Password")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        [Validations.ConfirmPasswordValidation(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _service.GetUserByEmail(EmailAddress);
            if (user == null)
            {
                ErrorMessage = "System dont have user with email like this.Please try again";
                return Page();
            }
            user.Password = Password;
            TempData["forgotPasswordUser"] = JsonDeserializeHelper.SerializeObject(user);

            return RedirectToPage("./ConfirmEmail", new { email = EmailAddress, action = "forgotPassword" });

        }
    }
}
