using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing Stocks entities
    /// </summary>
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);

        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);

        Task<List<BuyOrder>> GetBuyOrders();

        Task<List<SellOrder>> GetSellOrders();
    }
}
