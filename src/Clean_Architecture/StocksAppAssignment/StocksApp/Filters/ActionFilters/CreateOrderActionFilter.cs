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
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;

        public CreateOrderActionFilter(IStocksService stocksService, ILogger<CreateOrderActionFilter> logger, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, IFinnhubCompanyProfileService finnhubCompanyProfileService)
        {
            _stocksService = stocksService;
            _logger = logger;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
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
                        await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                    Dictionary<string, object?>? stockPriceQuoteDictionary =
                        await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

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
