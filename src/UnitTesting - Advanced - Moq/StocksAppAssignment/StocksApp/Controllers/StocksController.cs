using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public StocksController(IStocksService stocksService, IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _stocksService = stocksService;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [Route("[action]/{stock?}")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? searchBy, string? stock, bool? showAll)
        {
            if (stock != null) ViewBag.stock = stock;

            List<Dictionary<string, string>>? stockResults = new List<Dictionary<string, string>>();
            List<string>? popularStocks = _tradingOptions.Value.Top25PopularStocks?.Split(",").ToList();

            if (string.IsNullOrEmpty(searchBy))
            {
                stockResults = await _finnhubService.GetStocks();
            }
            else
            {
                Dictionary<string, object>? stocksFromSearchStocks = await _finnhubService.SearchStocks(searchBy);
                if (stocksFromSearchStocks != null)
                {
                    stockResults =
                        JsonSerializer.Deserialize<List<Dictionary<string, string>>?>(stocksFromSearchStocks["result"]
                            .ToString() ?? string.Empty);
                }
            }
            List<Stock> stocks = new List<Stock>();
            if (stockResults != null)
            {
                stockResults = stockResults.FindAll(temp => popularStocks != null && popularStocks.Contains(temp["displaySymbol"])); 
                foreach (var obj in stockResults)
                {
                    stocks.Add(new Stock()
                    {
                        StockName = obj["description"],
                        StockSymbol = obj["displaySymbol"]
                    });
                }

            }

            return View(stocks);
        }
    }
}
