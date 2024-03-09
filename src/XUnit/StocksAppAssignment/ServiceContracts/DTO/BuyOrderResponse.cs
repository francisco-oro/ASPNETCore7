using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as a return type for most BuyOrderService methods
    /// </summary>
    public class BuyOrderResponse
    {
        public Guid? BuyOrderID;
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public int? Quantity { get; set; }
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

            if (obj.GetType() != typeof(BuyOrderResponse))
            {
                return false;
            }

            BuyOrderResponse? other = obj as BuyOrderResponse;
            return this.BuyOrderID == other.BuyOrderID &&
                   this.StockSymbol == other.StockSymbol;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class BuyOrderExtensions
    {
        // Converts from BuyOrder object to BuyOrderResponse object
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrderResponse buyOrderResponse)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrderResponse.BuyOrderID,
                StockSymbol = buyOrderResponse.StockSymbol,
                Price = buyOrderResponse.Price,
                StockName = buyOrderResponse.StockName,
                DateAndTimeOfOrder = buyOrderResponse.DateAndTimeOfOrder,
                Quantity = buyOrderResponse.Quantity,
                TradeAmount = Convert.ToDouble(buyOrderResponse.Quantity) * buyOrderResponse.Price
            };
        }
    }
}
