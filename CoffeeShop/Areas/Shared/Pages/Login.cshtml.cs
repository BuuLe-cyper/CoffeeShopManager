using AutoMapper;
using BussinessObjects.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CoffeeShop.Areas.Shared.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;
        public LoginModel(IMapper mapper, IUserService service)
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
        public string ReturnUrl { get; set; } = null;
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var userDTO = await _service.Login(UserName, Password);
                if (userDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,UserName),
                         new Claim("userId", userDTO.UserID.ToString())
                    };

                    if (userDTO.AccountType == 1)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return Redirect(returnUrl);
                    //return RedirectToPage("/Index");
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            else
                return Page();
        }
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the login page or home page
            return RedirectToPage("/Index"); // Adjust the redirect path as necessary
        }
    }
}
