namespace StocksApp.Core.ServiceContracts
{
    /// <summary>
    /// Represents the business logic for retrieving data from Finnhub
    /// </summary>
    public interface IFinnhubCompanyProfileService
    {
        /// <summary>
        /// Returns the essential information about a specific company
        /// </summary>
        /// <param name="stockSymbol">Stock Symbol</param>
        /// <returns>A dictionary with information about the stock company profile: 
        /// country - Country of company's headquarter 
        /// currency - Currency used in company filings
        /// exchange - Listed exchange
        /// finnhubIndustry - Finnhub industry clasification
        /// ipo - IPO date
        /// logo - Logo image
        /// marketCapitalization - Market Capitalization
        /// name - Company name. 
        /// </returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string? stockSymbol);
    }
}
