using StocksApp.Core.Domain.RepositoryContracts.ServiceContracts;
using StocksApp.Core.ServiceContracts;

namespace StocksApp.Core.Services
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRespository _finnhubRespository;

        public FinnhubCompanyProfileService(IFinnhubRespository finnhubRespository)
        {
            _finnhubRespository = finnhubRespository;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }


            Dictionary<string, object>? companyProfile = await _finnhubRespository.GetCompanyProfile(stockSymbol);
            if (companyProfile == null)
            {
                return null;
            }

            return companyProfile;
        }
    }
}
