using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.Services;
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
        private const string ENDPOINT = "User/";
        private readonly IMapper _mapper;
        private readonly ApiClientService _service;
        public LoginModel(IMapper mapper, ApiClientService service)
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
        public async Task<IActionResult> OnGetAsync(string ReturnUrl = null)
        {

            TempData["returnURL"] = ReturnUrl;
            return Page();
        }

        public async Task OnLoadAuthenticationAsync()
        {
            var rmUserID = Request.Cookies["RmLoginUserId"];
            if (string.IsNullOrEmpty(rmUserID))
                return;
            var result = await _service.GetAsync<UsersDTO>(ENDPOINT + rmUserID);
            if (!result.IsSuccess)
                return;
            var userDTO = result.Data;
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
                var res = await _service.PostAsync<LoginRes>(ENDPOINT+"login",new LoginVM(UserName, Password));
                if (!res.IsSuccess)
                {
                    ErrorMessage = res.ErrorMessage ?? "Login failed!";
                    return Page();
                }                   
                var data = res.Data;
                if (data != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, data.Username),
                        new Claim("userId", data.UserId.ToString())
                    };

                    string areaName = "";
                    string pageName = "/Index";

                    claims.Add(new Claim(ClaimTypes.Role, data.Role));
                    areaName = data.Role; // Specify the Admin area

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    var userDTO = await _service.GetAsync<UsersDTO>(ENDPOINT);
                    if (RememberMe)
                    {
                        CookieOptions options = new()
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(7), // Set expiration for 1 week
                            HttpOnly = true
                        };
                        Response.Cookies.Append("RmLoginUserId", data.UserId.ToString(), options);
                    }

                    LoginSessionConfigure(userDTO.Data);
                    HttpContext.Session.SetString("AuthToken", data.Token);
                    if (TempData["returnURL"] != null)
                    {
                        string? pageUrl = TempData["returnURL"] as string;
                        return Redirect(pageUrl);

                    }

                    return RedirectToPage(pageName, new { area = areaName });
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
            //Delete cookie
            Response.Cookies.Delete("RmLoginUserId");
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
