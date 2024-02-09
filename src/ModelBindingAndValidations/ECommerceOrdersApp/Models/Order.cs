using System.ComponentModel.DataAnnotations;
using ECommerceOrdersApp.CustomValidators;

namespace ECommerceOrdersApp.Models
{
    public class Order
    {
        public int? OrderNo { get; set; }

        
        [Required(ErrorMessage = "{0} can't be blank")]
        [MinimumOrderDateValidator(ErrorMessage = "{0} should be greater than or equal to {1}")]
        public DateTime OrderDate { get; set; }

        [RegularExpression("^\\d*\\.*\\d*$", ErrorMessage = "{0} Must be a valid double number")]
        [Required(ErrorMessage = "{0} can't be blank")]
        public double InvoicePrice { get; set; }

        [InvoicePriceValidator("InvoicePrice", ErrorMessage = "InvoicePrice doesn't match with the total cost of the specified products in the order.")]
        [Required(ErrorMessage = "{0} can't be blank")]
        [ProductsLengthValidator(ErrorMessage = "{0} should have at least 1 product")]
        public List<Product?> Products { get; set; } = new List<Product?>();
    }
}
