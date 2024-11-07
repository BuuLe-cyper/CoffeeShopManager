using BusinessObjects.Services;
using BussinessObjects.Services;
using CoffeeShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace CoffeeShop.Areas.Shared.Pages
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly MailService _mailService;
        private readonly IUserService _userService;

        public ConfirmEmailModel(MailService mailService, IUserService userService)
        {
            _mailService = mailService;
            _userService = userService;
        }

        public string ErrorMessage { get; set; } = string.Empty;
        public string? OTP { get; set; }
        public DateTime ExpiredTime { get; set; }

        public async Task<IActionResult> OnGet(string userEmail)
        {
            // Check if OTP and expiration time are already set in session
            OTP = HttpContext.Session.GetString("OTP");
            var expiredTime = HttpContext.Session.GetString("ExpiredTime");
            ExpiredTime = string.IsNullOrEmpty(expiredTime) ? DateTime.MinValue : DateTime.Parse(expiredTime);

            // Check if the OTP has expired
            if (ExpiredTime <= DateTime.UtcNow && string.IsNullOrEmpty(expiredTime))
            {
                ErrorMessage = "OTP has expired. Please request a new one.";
                return Page(); // Return early if the OTP is expired
            }

            if (string.IsNullOrEmpty(OTP))
            {
                // Generate new OTP and expiration time if not already set
                OTP = GenerateOTP();
                ExpiredTime = DateTime.UtcNow.AddMinutes(3);

                // Store them in session
                HttpContext.Session.SetString("OTP", OTP);
                HttpContext.Session.SetString("ExpiredTime", ExpiredTime.ToString("o")); // Store as ISO 8601 format

                // Send the OTP via email to the user
                await _mailService.SendEmailAsync(userEmail, "Your OTP", $"Your OTP is: {OTP}");
            }

            return Page();
        }

        private string GenerateOTP()
        {
            int otp = new Random().Next(100000, 999999);
            return otp.ToString();
        }
    }
}
