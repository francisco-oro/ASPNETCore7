using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid? SellOrderID;
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }


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

    public static class SellOrderExtensions
    {
        // Converts from SellOrder object to SellOrderResponse object
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                Price = sellOrder.Price,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                TradeAmount = Convert.ToDouble(sellOrder.Quantity) * sellOrder.Price

            };
        }
    }
}
