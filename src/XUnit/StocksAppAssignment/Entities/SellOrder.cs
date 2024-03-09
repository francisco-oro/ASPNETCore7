using System.ComponentModel.DataAnnotations;


namespace Entities
{
    /// <summary>
    /// Domain model for selling an order
    /// </summary>
    public class SellOrder
    {
        public Guid? SellOrderID;
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime? DateAndTimeOfOrder { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
