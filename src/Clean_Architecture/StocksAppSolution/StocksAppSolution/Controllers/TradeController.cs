using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using StocksApp.Core.DTO;
using StocksApp.Core.ServiceContracts;
using StocksApp.UI.Filters.ActionFilters;
using StocksApp.UI.Models;

namespace StocksApp.UI.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, IOptions<TradingOptions> tradingOptions, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _tradingOptions = tradingOptions;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol?}/")]
        public async Task<IActionResult> Index(string? stockSymbol = "MSFT")
        {
            _logger.LogDebug($"Index action method from trade controller. stockSymbol = {stockSymbol}");
            Dictionary<string, object>? companyProfileDictionary = 
                await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object?>? stockPriceQuoteDictionary = 
                await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                Price = Convert.ToDouble(stockPriceQuoteDictionary?["c"].ToString()),
                StockName = companyProfileDictionary?["name"].ToString(),
                StockSymbol = companyProfileDictionary?["ticker"].ToString()
            }; 
            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            _logger.LogDebug($"BuyOrder action method from trade controller. orderRequest = {orderRequest.StockName}: {orderRequest.Quantity}");

            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            await _stocksService.CreateBuyOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            _logger.LogDebug($"SellOrder action method from trade controller. orderRequest = {orderRequest.StockName}: {orderRequest.Quantity}");

            orderRequest.DateAndTimeOfOrder = DateTime.Now;

            await _stocksService.CreateSellOrder(orderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            _logger.LogDebug("Orders action method from tradeController");
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Orders orders = new Orders()
            {
                BuyOrders = buyOrderResponses,
                SellOrders = sellOrderResponses
            };
            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            _logger.LogDebug("OrdersPDF action method from tradeController");

            List<BuyOrderResponse>? buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse>? sellOrder = await _stocksService.GetSellOrders();
            Orders orders = new Orders() { BuyOrders = buyOrders, SellOrders = sellOrder };
            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20},
                PageOrientation = Orientation.Landscape
            };
        }


        [Route("/api/v1/StockPriceQuote")]
        public async Task<IActionResult> GetStockPriceQuote([FromQuery] string? stockSymbol)
        {
            _logger.LogDebug("GetStockPriceQuote action method from tradeController");

            if (stockSymbol == null)
            {
                return NotFound();
            }

            Dictionary<string, object?>? companyProfileDictionary = 
                await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

            return Json(companyProfileDictionary);
        }
    }
}
