using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Validations
{
    public class ConfirmPasswordValidation:ValidationAttribute
    {
        private readonly string _passwordPropertyName;

        public ConfirmPasswordValidation(string passwordPropertyName)
        {
            _passwordPropertyName = passwordPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var passwordProperty = validationContext.ObjectType.GetProperty(_passwordPropertyName);
            if (passwordProperty == null) 
                return new ValidationResult($"Unknown property :{_passwordPropertyName}.");
            var password = passwordProperty.GetValue(validationContext.ObjectInstance,null) as string;
            var confirmPassword = value as string;
            if (password != confirmPassword)
                return new ValidationResult("Confirm password do not match");
            return ValidationResult.Success;
        }
    }
}
