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
                        StockSymbol = "GGP", StockName =GGP Inc.,1/1/2024,96,25.73
                    new BuyOrder() { BuyOrderID = Guid.Parse("876177C0-CCA7-4BEA-B21E-DCE33825D91B"),
                        StockSymbol = "PEO", StockName =Adams Natural  Resources Fund, Inc.,10/9/2023,97,15.08
                    new BuyOrder() { BuyOrderID = Guid.Parse("B5FF3BA9-602C-41D2-97F2-3FCAD7786422"),
                        StockSymbol = "SGU", StockName =Star Gas Partners, L.P.,7/14/2023,62,64.7
                    new BuyOrder() { BuyOrderID = Guid.Parse("A7B27A7C-4FE3-442C-9D8B-E60D6473C7A5"),
                        StockSymbol = "BXE", StockName =Bellatrix Exploration Ltd,12/3/2023,100,3.65
                });

                _sellOrders.AddRange(new List<SellOrder>()
                {
                    "2B36EDE5-0099-488D-BAFD-B57347CF1664",AEUA,Anadarko Petroleum Corporation,11/7/2023,67,17.71
                    "257181D2-98E2-427D-A8B3-DD043A2B650E",FONR,Fonar Corporation,12/6/2023,21,29.42
                    "7EE87200-2976-4C13-9AF8-AE86F2DD358C",FBZ,First Trust Brazil AlphaDEX Fund,4/11/2023,15,14.37
                    "C1AF3B50-6DC0-4FA4-9F84-AE357E0C7852",CACG,ClearBridge All Cap Growth ETF,12/24/2023,38,88.730

                });
            }
        }

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            throw new NotImplementedException();
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
