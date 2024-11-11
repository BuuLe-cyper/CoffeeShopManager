using BusinessObjects.Services;
using BussinessObjects.DTOs;
using BussinessObjects.Services;
using CoffeeShop.Helper;
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

        public async Task<IActionResult> OnGetAsync(string email, string action)
        {
            // Check if OTP and expiration time are already set in session
            OTP = HttpContext.Session.GetString("OTP");
            var expiredTime = HttpContext.Session.GetString("ExpiredTime");
            ExpiredTime = string.IsNullOrEmpty(expiredTime) ? DateTime.MinValue : DateTime.Parse(expiredTime);
            TempData["action"] = action;


            if (string.IsNullOrEmpty(OTP))
            {
                OTP = GenerateOTP();
                ExpiredTime = DateTime.UtcNow.AddMinutes(3);

                HttpContext.Session.SetString("OTP", OTP);
                HttpContext.Session.SetString("ExpiredTime", ExpiredTime.ToString("o")); // Store as ISO 8601 format

                await SendMailAsync(email, OTP, action);
            }
            // Check if the OTP has expired
            if (ExpiredTime <= DateTime.UtcNow && string.IsNullOrEmpty(expiredTime))
            {
                ErrorMessage = "OTP has expired. Please request a new one.";
                return Page(); // Return early if the OTP is expired
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            string? action = TempData["action"] as string;
            if (action == null)
                return Page();
            if (action.Equals("register"))
            {
                var ruJson = TempData["RegisterUser"] as string;
                var registerUser = JsonDeserializeHelper.DeserializeObject<UserVM>(ruJson);
                if (registerUser == null)
                    return BadRequest();
                await _userService.Register(registerUser.Username, registerUser.Password, registerUser.Email);
                return RedirectToPage("./Login");
            }
            if (action.Equals("forgotPassword"))
            {
                var fuJson = TempData["forgotPasswordUser"] as string;
                var forgotPassUser = JsonDeserializeHelper.DeserializeObject<UsersDTO>(fuJson);
                if (forgotPassUser == null) return BadRequest();
                await _userService.UpdateUser(forgotPassUser);
                return RedirectToPage("./Login");
            }
            return Page();
        }
        private static string GenerateOTP()
        {
            int otp = new Random().Next(100000, 999999);
            return otp.ToString();
        }

        private async Task SendMailAsync(string email, string OTP, string action)
        {
            string subject = "";
            string content = "";

            if (action == "register")
            {
                subject = "Welcome! Confirm Your Registration";
                content = $"Thank you for registering! Your OTP is: {OTP}. Please use this OTP to complete your registration.";
            }
            else if (action == "forgotPassword")
            {
                subject = "Password Reset Request";
                content = $"We received a request to reset your password. Your OTP is: {OTP}. Use this OTP to reset your password.";
            }

            await _mailService.SendEmailAsync(email, subject, content);
        }

    }
}
