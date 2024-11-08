using AutoMapper;
using BussinessObjects.DTOs;
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
    [IgnoreAntiforgeryToken]
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
        public async Task<IActionResult> OnGetAsync()
        {
            var rmUserId = Request.Cookies["RmLoginUserId"];
            if (!string.IsNullOrEmpty(rmUserId))
            {
                var userDTO = await _service.GetUser(Guid.Parse(rmUserId));
                if (userDTO == null) return Page();

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
                LoginSessionConfigure(userDTO);
                return RedirectToPage("/Index");
            }
            else
                Response.Cookies.Delete("RmLoginUserId");

            return Page();
        }

        public async Task OnLoadAuthenticationAsync()
        {
            var rmUserID = Request.Cookies["RmLoginUserId"];
            if (string.IsNullOrEmpty(rmUserID))
                return;
            var userDTO = await _service.GetUser(Guid.Parse(rmUserID));
            if (userDTO == null) return;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, UserName),
                new(ClaimTypes.Role, userDTO.AccountType == 1 ? "Admin" : "User")
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            LoginSessionConfigure(userDTO);
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
                        new Claim(ClaimTypes.Name, UserName),
                        new Claim("userId", userDTO.UserID.ToString())
                    };

                    string areaName;
                    string pageName = "/Index";

                    if (userDTO.AccountType == 1)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        areaName = "Admin"; // Specify the Admin area
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        areaName = "Customer"; // Specify the Customer area
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

                    LoginSessionConfigure(userDTO);

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

        private void LoginSessionConfigure(UsersDTO userDTO)
        {
            HttpContext.Session.SetString("IsLogin", "true");
            HttpContext.Session.SetString("User", JsonDeserializeHelper.SerializeObject(_mapper.Map<UserVM>(userDTO)));
        }

    }
}
