using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksApp.Models;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IStocksService stocksService, IFinnhubService finnhubService)
        {
            _stocksService = stocksService;
            _finnhubService = finnhubService;
        }

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? searchBy)
        {
            List<Dictionary<string, string>>? stockResults = new List<Dictionary<string, string>>();
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
