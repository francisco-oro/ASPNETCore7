using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for buying an order
    /// </summary>
    public class BuyOrder
    {
        public Guid? BuyOrderID;
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
