using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Pages.User
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
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
        public void OnGet()
        {
        }
    }
}
