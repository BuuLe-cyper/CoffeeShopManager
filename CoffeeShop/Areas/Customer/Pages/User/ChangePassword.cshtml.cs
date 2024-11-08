using AutoMapper;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Areas.Customer.Pages.User
{
    [Authorize(Roles = "User")]
    [BindProperties]
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        public ChangePasswordModel(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        public string? ErrorMessage { get; set; }

        [Length(6, 15)]
        [Display(Name = "Old password")]
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Length(6, 15)]
        [Display(Name = "New password")]
        [Required]
        [Validations.PasswordStrength]
        public string NewPass { get; set; } = string.Empty;

        [Length(6, 15)]
        [Required]
        [Display(Name = "Confirm password")]

        [Validations.ConfirmPasswordValidation(nameof(NewPass))]
        public string ConfirmPass { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }
            string? userJson = HttpContext.Session.GetString("User");
            if (userJson == null)
                return RedirectToPage("/Login", new { area = "Shared" });
            var userObject = JsonDeserializeHelper.DeserializeObject<UserVM>(userJson);
            if (userObject == null)
            {
                ErrorMessage = "Some error has occour. We cant confirm your old password, please try again";
                return RedirectToPage();
            }
            if (userObject.Password != OldPassword)
            {
                ErrorMessage = "Old password incorrect! Please try again carefully!";
                return RedirectToPage();
            }
            userObject.Password = NewPass;
            try
            {
                await _service.UpdateUser(_mapper.Map<UsersDTO>(userObject));
            }
            catch (Exception)
            {
                ErrorMessage = "Some error has occour. We cant confirm your old password, please try again";
            }
            return RedirectToPage("./UserProfile");

        }
    }
}
