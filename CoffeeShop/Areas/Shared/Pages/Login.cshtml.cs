using AutoMapper;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
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

        [BindProperty]
        public bool RememberMe { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
        public async void OnGet()
        {
            var rmUserId = Request.Cookies["RmLoginUserId"];
            if (!string.IsNullOrEmpty(rmUserId))
            {
                var userDTO = await _service.GetUser(Guid.Parse(rmUserId));
                if (userDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name,UserName),
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
                }
            }
            else
                Response.Cookies.Delete("RmLoginUserId");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ErrorMessage = "";
                var userDTO = await _service.Login(UserName, Password);
                if (userDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name,UserName),
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
                    
                    if (RememberMe)
                    {
                        CookieOptions options = new()
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(7), // Set expiration for 1 week
                            HttpOnly = true
                        };
                        Response.Cookies.Append("RmLoginUserId", userDTO.UserID.ToString(), options);
                    }

                    HttpContext.Session.SetString("IsLogin", "true");
                    HttpContext.Session.SetString("User", JsonDeserializeHelper.SerializeObject(_mapper.Map<UserVM>(userDTO)));

                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Username or Password invalid! Please try again";
                    return Page();
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
