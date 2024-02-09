using System.ComponentModel.DataAnnotations;

namespace ECommerceOrdersApp.CustomValidators
{
    public class MinimumOrderDateValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                DateTime OrderDate = Convert.ToDateTime(value);
                DateTime FromDate = DateTime.Parse("2000-01-01");

                if (OrderDate >= FromDate)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(string.Format(ErrorMessage, validationContext.MemberName, FromDate));
            }
            catch (Exception e)
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.MemberName));
            }
        }
    }
}
