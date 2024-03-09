using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating BuyOrder and SellOrder entities 
    /// </summary>
    public interface IStocksService
    {
        /// <summary>
        /// Adds a new BuyOrder into the list of BuyOrders
        /// </summary>
        /// <param name="request">Buy Order to add</param>
        /// <returns>Returns the same BuyOrder details, along with newly generated BuyOrderID</returns>
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request);

        /// <summary>
        /// Adds a new SellOrder into the list of SellOrders
        /// </summary>
        /// <param name="request">Sell Order to add</param>
        /// <returns>Returns the same BuyOrder SellOrder details, along with newly generated SellOrderID</returns>
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request);
        Task<List<BuyOrderResponse>> GetBuyOrders();
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
