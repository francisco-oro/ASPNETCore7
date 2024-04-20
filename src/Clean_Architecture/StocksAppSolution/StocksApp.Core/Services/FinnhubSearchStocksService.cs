using StocksApp.Core.Domain.RepositoryContracts.ServiceContracts;
using StocksApp.Core.ServiceContracts;

namespace StocksApp.Core.Services
{
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        private readonly IFinnhubRespository _finnhubRespository;

        public FinnhubSearchStocksService(IFinnhubRespository finnhubRespository)
        {
            _finnhubRespository = finnhubRespository;
        }
        public async Task<Dictionary<string, object>?> SearchStocks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            Dictionary<string, object>? matchingStocks = await _finnhubRespository.SearchStocks(query);
            if (matchingStocks != null) return matchingStocks;
            return null;
        }
    }
}
