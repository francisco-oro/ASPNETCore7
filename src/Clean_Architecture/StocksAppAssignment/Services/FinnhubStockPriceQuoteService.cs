using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using RepositoryContracts.ServiceContracts;
using ServiceContracts;

namespace Services
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
