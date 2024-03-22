using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _db;

        public StocksService(StockMarketDbContext db)
        {
            _db = db;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ValidationHelper.ModelValidation(request);

            BuyOrder buyOrder = request.ToBuyOrder();

            buyOrder.BuyOrderID = Guid.NewGuid();
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ValidationHelper.ModelValidation(request);

            SellOrder sellOrder = request.ToSellOrder();
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return await _db.BuyOrders.Select(order => order.ToBuyOrderResponse()).ToListAsync();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return await _db.SellOrders.Select(order => order.ToSellOrderResponse()).ToListAsync();
        }
    }
}
