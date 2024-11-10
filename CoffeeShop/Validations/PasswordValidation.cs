using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CoffeeShop.Validations
{
    public class PasswordStrengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Password must be 8–15 characters long, contain uppercase and lowercase letters, and at least one number or special character.";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string password)
                return ValidationResult.Success;

            // Define regex pattern for password validation
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d\W]).{6,15}$");

            if (!passwordRegex.IsMatch(password))
                return new ValidationResult(DefaultErrorMessage);

            return ValidationResult.Success;
        }
    }
}
