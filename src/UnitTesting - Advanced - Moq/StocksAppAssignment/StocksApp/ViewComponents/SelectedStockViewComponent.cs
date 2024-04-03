using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksApp.ViewComponents
{
    [ViewComponent]
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService)
        {
            _tradingOptions = tradingOptions;
            _stocksService = stocksService;
            _finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return Content("");
            }

            Dictionary<string, object>? companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
            if (companyProfileDictionary == null)
            {
                return Content(""); 
            }
            Dictionary<string, object?>? stockPriceQuoteDictionary =
                await _finnhubService.GetStockPriceQuote(stockSymbol);
            
            companyProfileDictionary.Add("price", stockPriceQuoteDictionary?["c"] ?? "Price is unavailable");
            return View(companyProfileDictionary);
        }
    }
}
