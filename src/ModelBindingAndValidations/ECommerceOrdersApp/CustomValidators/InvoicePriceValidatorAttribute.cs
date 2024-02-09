using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ECommerceOrdersApp.Models;

namespace ECommerceOrdersApp.CustomValidators
{
    public class InvoicePriceValidatorAttribute : ValidationAttribute
    {
        public string InvoicePriceName { get; set; }

        public InvoicePriceValidatorAttribute(string invoicePriceName)
        {
            InvoicePriceName = invoicePriceName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                List<Product> Products = value as List<Product>;

                PropertyInfo? InvoiceProperty = validationContext.ObjectType.GetProperty(InvoicePriceName);
                if (InvoiceProperty != null)
                {
                    double ExpectedPrice = 0f;
                    double InvoicePrice = Convert.ToDouble(InvoiceProperty.GetValue(validationContext.ObjectInstance));

                    foreach (Product product in Products)
                    {
                        ExpectedPrice += product.Quantity * product.Price;
                    }

                    if (InvoicePrice != ExpectedPrice)
                    {
                        return new ValidationResult(ErrorMessage,
                            new string[] { InvoicePriceName });
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }

            return null;
        }
    }
}
