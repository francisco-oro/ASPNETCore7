using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class StocksService : IStocksService
    {
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            throw new NotImplementedException();
        }
    }
}
