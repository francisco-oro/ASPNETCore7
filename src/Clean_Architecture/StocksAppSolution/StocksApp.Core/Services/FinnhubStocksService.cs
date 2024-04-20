using StocksApp.Core.Domain.RepositoryContracts.ServiceContracts;
using StocksApp.Core.ServiceContracts;

namespace StocksApp.Core.Services
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRespository _finnhubRespository;

        public FinnhubStocksService(IFinnhubRespository finnhubRespository)
        {
            _finnhubRespository = finnhubRespository;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var stocks = await _finnhubRespository.GetStocks();
            if (stocks != null) return stocks.ToList();
            return null;
        }
    }
}
