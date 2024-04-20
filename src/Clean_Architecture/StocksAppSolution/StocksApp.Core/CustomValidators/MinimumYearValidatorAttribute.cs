using System.ComponentModel.DataAnnotations;

namespace StocksApp.Core.CustomValidators
{
    internal class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int MinimumYear { get; set; } = 2000;
        private string DefaultErrorMessage { get; set; } = "Should not be older than Jan 01 {0}";
        
        // parameter less constructor
        public MinimumYearValidatorAttribute() { }

        // parametrized constructor
        public MinimumYearValidatorAttribute(int minimumYear)
        {
            MinimumYear = minimumYear;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = (DateTime)value;
                if (date.Year <= MinimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
                }
            }
            else
            {
                return ValidationResult.Success;
            }

            return null;
        }
    }
}
