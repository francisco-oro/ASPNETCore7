using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        public TradeController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions;
            _finnhubService = finnhubService;
            _stocksService = stocksService;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            if (_tradingOptions.Value.DefaultStockSymbol == null)
            {
                _tradingOptions.Value.DefaultStockSymbol = "MSFT";
            }
            Dictionary<string, object> companyProfileDictionary = 
                await _finnhubService.GetCompanyProfile(_tradingOptions.Value.DefaultStockSymbol);
            Dictionary<string, object> stockPriceQuoteDictionary = 
                await _finnhubService.GetStockPriceQuote(_tradingOptions.Value.DefaultStockSymbol);

            StockTrade stockTrade = new StockTrade()
            {
                Price = Convert.ToDouble(stockPriceQuoteDictionary["c"].ToString()),
                StockName = companyProfileDictionary["name"].ToString(),
                StockSymbol = companyProfileDictionary["ticker"].ToString()
            };
            return View(stockTrade);
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Index");
            }

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View("Index");
            }

            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Orders orders = new Orders()
            {
                BuyOrders = buyOrderResponses,
                SellOrders = sellOrderResponses
            };
            return View(orders);
        }

        [Route("api/v1/StockPriceQuote")]
        public async Task<IActionResult> GetStockPriceQuote([FromQuery] string stockSymbol)
        {
            if (stockSymbol == null)
            {
                return NotFound();
            }

            Dictionary<string, object> companyProfileDictionary =
                await _finnhubService.GetStockPriceQuote(stockSymbol);

            return Json(companyProfileDictionary);
        }
    }
}
