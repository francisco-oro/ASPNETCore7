using System.ComponentModel.DataAnnotations;
using System.Numerics;
using OrdersWebAPI.CustomValidators;

namespace OrdersWebAPI.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        public string? OrderNumber { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(50, ErrorMessage = "Maximum length of {0} is 50 characters")]
        public string CustomerName { get; set; }

        public DateTime? OrderDate { get; set; }

        [PositiveNumber]
        public double? TotalAmount { get; set; }

        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
