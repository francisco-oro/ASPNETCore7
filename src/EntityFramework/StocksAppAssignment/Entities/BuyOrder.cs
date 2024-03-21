﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for buying an order
    /// </summary>
    public class BuyOrder
    {
        [Key]
        public Guid? BuyOrderID;

        [StringLength(10)]
        public string? StockSymbol { get; set; }
        
        [StringLength(40)]
        public string? StockName { get; set; }
        
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
    }
}
