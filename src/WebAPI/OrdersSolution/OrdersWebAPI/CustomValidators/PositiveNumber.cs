using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace OrdersWebAPI.CustomValidators
{
    public class PositiveNumberAttribute : ValidationAttribute
    {
        public string DefaultErrorMessage { get; set; } = "Should be a positive number";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if (value is int i)
                {
                    if (i < 0)
                    {
                        return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
                    }
                    return ValidationResult.Success;
                }

                if (value is double number)
                {
                    if (number < 0.0)
                    {
                        return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
                    }
                    return ValidationResult.Success;
                }

            }

            return null;
        }
    }
}
