using StocksApp.Core.Domain.RepositoryContracts.ServiceContracts;
using StocksApp.Core.ServiceContracts;

namespace StocksApp.Core.Services
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRespository _finnhubRespository;

        public FinnhubStockPriceQuoteService(IFinnhubRespository finnhubRespository)
        {
            _finnhubRespository = finnhubRespository;
        }

        public async Task<Dictionary<string, object?>?> GetStockPriceQuote(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }

            Dictionary<string, object?>? priceQuote = await _finnhubRespository.GetStockPriceQuote(stockSymbol);
            if (priceQuote == null)
            {
                return null;
            }
            return priceQuote;

        }
    }
}
