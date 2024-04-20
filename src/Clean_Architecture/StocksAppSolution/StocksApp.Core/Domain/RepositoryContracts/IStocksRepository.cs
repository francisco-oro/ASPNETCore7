using StocksApp.Core.Domain.Entities;

namespace StocksApp.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Stocks entities
    /// </summary>
    public interface IStocksRepository
    {
        /// <summary>
        /// Adds a BuyOrder object to the data store
        /// </summary>
        /// <param name="buyOrder">BurOrder object to add</param>
        /// <returns>Returns the BuyOrder object after adding it to the table</returns>
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        /// <summary>
        /// Adds a SellOrder object to the data store
        /// </summary>
        /// <param name="sellOrder">SellOrder object to add</param>
        /// <returns>Returns the SellOrder object after adding it to the table</returns>
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        /// <summary>
        /// Returns all BuyOrders in the data store
        /// </summary>
        /// <returns>A list of BuyOrder objects from table</returns>
        Task<List<BuyOrder>> GetBuyOrders();

        /// <summary>
        /// Returns all SellOrders in the data store 
        /// </summary>
        /// <returns>A list of SellOrder objects from table</returns>
        Task<List<SellOrder>> GetSellOrders();
    }
}
