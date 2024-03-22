namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for retrieving data from Finnhub
    /// </summary>
    public interface IFinnhubService
    {
        /// <summary>
        /// Returns the information about an specific stock's price
        /// </summary>
        /// <param name="stockSymbol">Stock Symbol</param>
        /// <returns>A dictionary with information about the stock price: 
        /// c - current Price
        /// d - Change
        /// dp - Percent change
        /// h - High price of the day
        /// l - Low price of the day
        /// o - Open price of the day
        /// pc - Previous close price
        /// </returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

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
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}
