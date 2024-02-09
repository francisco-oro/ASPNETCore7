using System.ComponentModel.DataAnnotations;
using ECommerceOrdersApp.Models;

namespace ECommerceOrdersApp.CustomValidators
{
    public class ProductsLengthValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            List<Product> products = value as List<Product>;
            if (products.Count > 1)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(ErrorMessage, validationContext.MemberName));
        }
    }
}
