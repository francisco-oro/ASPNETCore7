using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid? SellOrderID;
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TrradeAmount { get; set; }


        // It compares the current object to another object of CurrentResponse type and returns true, if both values are the same;
        // Otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(SellOrderResponse))
            {
                return false;
            }

            SellOrderResponse? other = obj as SellOrderResponse;

            return this.SellOrderID == other.SellOrderID &&
                   this.StockSymbol == other.StockSymbol;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
