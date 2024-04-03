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
        public async Task<IActionResult> Explore(string? searchBy, string? stock, bool showAll = false)
        {
            if (stock != null) ViewBag.stock = stock;
            List<Stock> stocks = new List<Stock>();


            if (string.IsNullOrEmpty(searchBy))
            {
                List<string>? popularStocks = _tradingOptions.Value.Top25PopularStocks?.Split(",").ToList();

                var stockResults = await _finnhubService.GetStocks();
                if (stockResults != null)
                {
                    if (showAll == false)
                    {
                        stockResults = stockResults.FindAll(temp => popularStocks != null && popularStocks.Contains(temp["displaySymbol"]));
                    }

                    foreach (var obj in stockResults)
                    {
                        stocks.Add(new Stock
                        {
                            StockName = obj["description"],
                            StockSymbol = obj["displaySymbol"]
                        });
                    }

                }
            }

            else
            {
                Dictionary<string, object>? stocksFromSearchStocks = await _finnhubService.SearchStocks(searchBy);
                if (stocksFromSearchStocks != null)
                {
                    JsonElement stocksFromSearchJsonElement = (JsonElement)stocksFromSearchStocks["result"];
                    List<Dictionary<string, object>>? stocksFromSearchList =
                        JsonSerializer.Deserialize<List<Dictionary<string, object>>>(stocksFromSearchJsonElement.GetRawText());

                    if (stocksFromSearchList != null)
                        foreach (Dictionary<string, object> stockItem in stocksFromSearchList)
                        {
                            stocks.Add(new Stock() { StockName = stockItem["description"].ToString(), StockSymbol = stockItem["symbol"].ToString() });
                        }
                }
            }

            return View(stocks);

        }
    }
}
