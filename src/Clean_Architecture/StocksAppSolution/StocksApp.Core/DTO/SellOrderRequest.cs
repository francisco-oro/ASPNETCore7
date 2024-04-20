using System.ComponentModel.DataAnnotations;
using StocksApp.Core.CustomValidators;
using StocksApp.Core.Domain.Entities;


namespace StocksApp.Core.DTO
{
    /// <summary>
    /// DTO Class for adding a new Sell Order
    /// </summary>
    public class SellOrderRequest
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

        public SellOrder ToSellOrder()
        {
            return new SellOrder()
            { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
        }
    }
}
