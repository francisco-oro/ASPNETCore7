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
        private readonly IFinnhubStocksService _finnhubStocksService;
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubStocksService finnhubStocksService, IFinnhubSearchStocksService finnhubSearchStocksService, IOptions<TradingOptions> tradingOptions, ILogger<StocksController> logger)
        {
            _finnhubStocksService = finnhubStocksService;
            _finnhubSearchStocksService = finnhubSearchStocksService;
            _tradingOptions = tradingOptions;
            _logger = logger;
        }

        [Route("[action]/{stock?}")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? searchBy, string? stock, bool showAll = false, int pg=1)
        {
            if (stock != null) ViewBag.stock = stock;
            List<Stock> stocks = new List<Stock>();


            if (string.IsNullOrEmpty(searchBy))
            {
                _logger.LogDebug("Explore Action Method with searchBy");
                List<string>? popularStocks = _tradingOptions.Value.Top25PopularStocks?.Split(",").ToList();

                var stockResults = await _finnhubStocksService.GetStocks();
                if (stockResults != null)
                {
                    if (showAll == false)
                    {
                        stockResults = stockResults.FindAll(temp => popularStocks != null && popularStocks.Contains(temp["displaySymbol"]));
                    }
                    else
                    {
                        const int pageSize = 20;
                        if (pg < 1)
                        {
                            pg = 1;
                        }

                        int recsCount = stockResults.Count();

                        var pager = new Pager(recsCount, pg, pageSize);
                        int recSkip = (pg -1 ) * pageSize;

                        stockResults = stockResults.Skip(recSkip).Take(pager.PageSize).ToList();
                        ViewBag.Pager = pager;
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
                _logger.LogDebug("Explore Action Method without searchBy");

                Dictionary<string, object>? stocksFromSearchStocks = await _finnhubSearchStocksService.SearchStocks(searchBy);
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
