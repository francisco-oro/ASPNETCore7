using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Core.ServiceContracts;

namespace StocksApp.ViewComponents
{
    [ViewComponent]
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;

        public SelectedStockViewComponent(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService, IFinnhubCompanyProfileService finnhubCompanyProfileService)
        {
            _tradingOptions = tradingOptions;
            _stocksService = stocksService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return Content("");
            }

            try
            {
                Dictionary<string, object>? companyProfileDictionary =
                    await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol);
                if (companyProfileDictionary == null)
                {
                    return Content("");
                }

                Dictionary<string, object?>? stockPriceQuoteDictionary =
                    await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol);

                companyProfileDictionary.Add("price", stockPriceQuoteDictionary?["c"] ?? "Price is unavailable");
                return View(companyProfileDictionary);
            }
            catch (Exception ae)
            {
                return Content(ae.Message);
            }

        }
    }
}
