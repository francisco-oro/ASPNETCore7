using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class BuyOrder
    {
        public Guid BuyOrderID;
        [Required(ErrorMessage = "{0} is required")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string? StockName { get; set; }
        
        public DateTime? DateAndTimeOfOrder { get; set; }
        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000")]

        public int? Quantity { get; set; }
        [Range(1.0, 100000.0, ErrorMessage = "Value should be between 1 and 100000")]
        public double? Price { get; set; }
    }
}
