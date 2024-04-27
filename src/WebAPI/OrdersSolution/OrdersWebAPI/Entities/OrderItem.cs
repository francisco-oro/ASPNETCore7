using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrdersWebAPI.CustomValidators;

namespace OrdersWebAPI.Entities
{
    public class OrderItem
    {
        [Key] 
        public Guid OrderItemId { get; set; }

        //Uniqueidentifier
        [Required(ErrorMessage = "{0} is required")]
        public Guid? OrderId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [MaxLength(50, ErrorMessage = "Maximum length of {0} is 50 characters")]
        public string ProductName { get; set; }

        [PositiveNumber] 
        public int Quantity { get; set; }

        [PositiveNumber] 
        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
