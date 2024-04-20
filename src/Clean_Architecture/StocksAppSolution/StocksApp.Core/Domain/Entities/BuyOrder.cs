using System.ComponentModel.DataAnnotations;

namespace StocksApp.Core.Domain.Entities
{
    /// <summary>
    /// Domain Model for buying an order
    /// </summary>
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        [StringLength(10)]
        public string? StockSymbol { get; set; }
        
        [StringLength(40)]
        public string? StockName { get; set; }
        
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
