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
