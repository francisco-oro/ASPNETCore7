using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        private readonly IStocksService _stocksService;
        private readonly ILogger<CreateOrderActionFilter> _logger;
        private readonly IFinnhubService _finnhubService;

        public CreateOrderActionFilter(IStocksService stocksService, ILogger<CreateOrderActionFilter> logger, IFinnhubService finnhubService)
        {
            _stocksService = stocksService;
            _logger = logger;
            _finnhubService = finnhubService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                string? stockSymbol = string.Empty;
                var orderRequest = context.ActionArguments["orderRequest"];
                if (orderRequest?.GetType() == typeof(BuyOrderRequest))
                {
                    BuyOrderRequest buyOrderRequest = (BuyOrderRequest) orderRequest;
                    stockSymbol = buyOrderRequest.StockSymbol;
                }

                if (orderRequest?.GetType() == typeof(SellOrderRequest))
                {
                    SellOrderRequest sellOrderRequest = (SellOrderRequest) orderRequest;
                    stockSymbol = sellOrderRequest.StockSymbol;
                }

                if (!tradeController.ModelState.IsValid)
                {
                    tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

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

                    context.Result = tradeController.View("Index", stockTrade);
                }
            }
            await next();
        }
    }
}
