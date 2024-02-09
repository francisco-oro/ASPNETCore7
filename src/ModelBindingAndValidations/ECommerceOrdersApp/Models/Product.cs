using System.ComponentModel.DataAnnotations;

namespace ECommerceOrdersApp.Models
{
    public class Product
    {
        [RegularExpression("\\d*", ErrorMessage = "{0} should be a valid integer")]
        [Required(ErrorMessage = "{0} can't be blank")]
        public int ProductCode { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [RegularExpression("\\d*", ErrorMessage = "{0} should be a valid integer")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [RegularExpression("^\\d*\\.*\\d*$", ErrorMessage = "{0} Must be a valid double number")]
        public double Price { get; set; }
    }
}
