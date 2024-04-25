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
                if (value is int)
                {

                }

                if (value is double)
                {
                    
                }
                double number = (double)value;
                if (number < 0.0)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
                }
                return ValidationResult.Success;
            }

            return null;
        }
    }
}
