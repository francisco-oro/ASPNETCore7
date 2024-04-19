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
