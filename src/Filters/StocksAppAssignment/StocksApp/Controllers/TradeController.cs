using System.Reflection.Metadata;
using Entities;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;
using System.Text.Json;
using StocksApp.Filters.ActionFilters;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("[action]/{stockSymbol?}/")]
        public async Task<IActionResult> Index(string? stockSymbol = "MSFT")
        {
            _logger.LogDebug($"Index action method from trade controller. stockSymbol = {stockSymbol}");
            Dictionary<string, object>? companyProfileDictionary = 
                await _finnhubService.GetCompanyProfile(stockSymbol);
            Dictionary<string, object?>? stockPriceQuoteDictionary = 
                await _finnhubService.GetStockPriceQuote(stockSymbol);

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
                await _finnhubService.GetStockPriceQuote(stockSymbol);

            return Json(companyProfileDictionary);
        }
    }
}
