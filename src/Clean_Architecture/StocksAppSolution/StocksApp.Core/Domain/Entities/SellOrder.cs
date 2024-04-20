using System.ComponentModel.DataAnnotations;


namespace StocksApp.Core.Domain.Entities
{
    /// <summary>
    /// Domain model for selling an order
    /// </summary>
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [StringLength(10)]
        public string? StockSymbol { get; set; }

        [StringLength(40)]
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
