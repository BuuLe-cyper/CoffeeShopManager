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

namespace CoffeeShop.Pages.User
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
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userDTO = await _service.Login(UserName,Password);
                if(userDTO != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,UserName),
                    };

                    if(userDTO.AccountType == 1)
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
                    return RedirectToPage("/Index");
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            else
                return Page();
        }
    }
}
