using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
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

        [Route("Trade/Index")]
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

        [Route("trade/api/v1/StockPriceQuote")]
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
