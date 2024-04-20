using System.ComponentModel.DataAnnotations;
using StocksApp.Core.CustomValidators;
using StocksApp.Core.Domain.Entities;

namespace StocksApp.Core.DTO
{
    /// <summary>
    /// DTO class for adding a new Buy Order
    /// </summary>
    public class BuyOrderRequest
    {

        [Required(ErrorMessage = "{0} is required")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string? StockName { get; set; }

        [MinimumYearValidator(2000)]
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Value should be between 1 and 100000")]
        public uint? Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Value should be between 1 and 100000")]
        public double? Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                StockName = StockName,
                Quantity = Quantity,
                Price = Price,
                StockSymbol = StockSymbol,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };
        }
    }
}
