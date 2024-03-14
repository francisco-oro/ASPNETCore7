using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService(bool initialize = true)
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();

            if (initialize)
            {
                _buyOrders.AddRange(new List<BuyOrder>()
                {
                    new BuyOrder() { BuyOrderID = Guid.Parse("C9AE2F7F-29FB-4CB0-8C59-0A07062EB104"),
                        StockSymbol = "GGP", StockName = "GGP Inc.", DateAndTimeOfOrder = DateTime.Parse("1/1/2024"), Quantity = 96, Price = 25.73 },
                    new BuyOrder() { BuyOrderID = Guid.Parse("876177C0-CCA7-4BEA-B21E-DCE33825D91B"),
                        StockSymbol = "PEO", StockName = "Adams Natural  Resources Fund, Inc.", DateAndTimeOfOrder = DateTime.Parse("10/9/2023"), Quantity = 97, Price = 15.08 },
                    new BuyOrder() { BuyOrderID = Guid.Parse("B5FF3BA9-602C-41D2-97F2-3FCAD7786422"),
                        StockSymbol = "SGU", StockName = "Star Gas Partners, L.P.", DateAndTimeOfOrder = DateTime.Parse("7/14/2023"), Quantity = 62, Price = 64.70 },
                    new BuyOrder() { BuyOrderID = Guid.Parse("A7B27A7C-4FE3-442C-9D8B-E60D6473C7A5"),
                        StockSymbol = "BXE", StockName = "Bellatrix Exploration Ltd", DateAndTimeOfOrder = DateTime.Parse("12/3/2023"), Quantity = 100, Price = 3.65 },
                });

                _sellOrders.AddRange(new List<SellOrder>()
                {
                    new SellOrder() { SellOrderID = Guid.Parse("2B36EDE5-0099-488D-BAFD-B57347CF1664"), StockSymbol = "AEUA", StockName = "Anadarko Petroleum Corporation", DateAndTimeOfOrder = DateTime.Parse("11/7/2023"), Quantity = 67, Price = 17.71 }, 
                    new SellOrder() { SellOrderID = Guid.Parse("257181D2-98E2-427D-A8B3-DD043A2B650E"), StockSymbol = "FONR", StockName = "Fonar Corporation", DateAndTimeOfOrder = DateTime.Parse("12/6/2023"), Quantity = 21, Price = 29.42 }, 
                    new SellOrder() { SellOrderID = Guid.Parse("7EE87200-2976-4C13-9AF8-AE86F2DD358C"), StockSymbol = "FBZ", StockName = "First Trust Brazil AlphaDEX Fund", DateAndTimeOfOrder = DateTime.Parse("4/11/2023"), Quantity = 15, Price = 14.37 }, 
                    new SellOrder() { SellOrderID = Guid.Parse("C1AF3B50-6DC0-4FA4-9F84-AE357E0C7852"), StockSymbol = "CACG", StockName = "ClearBridge All Cap Growth ETF", DateAndTimeOfOrder = DateTime.Parse("12/24/2023"), Quantity = 38, Price = 88.730 }, 

                });
            }
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
            _buyOrders.Add(buyOrder);
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
            _sellOrders.Add(sellOrder);
            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            return _buyOrders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            return _sellOrders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
