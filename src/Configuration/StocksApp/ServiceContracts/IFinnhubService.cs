namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote (string stockSymbol);
        Task<Dictionary<string, string>?> GetCompanyProfile(string stockSymbol);
    }
}
