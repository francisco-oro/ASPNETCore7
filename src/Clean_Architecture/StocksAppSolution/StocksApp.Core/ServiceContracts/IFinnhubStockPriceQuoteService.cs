namespace StocksApp.Core.ServiceContracts
{
    /// <summary>
    /// Represents the business logic for retrieving data from Finnhub
    /// </summary>
    public interface IFinnhubStockPriceQuoteService
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
        Task<Dictionary<string, object?>?> GetStockPriceQuote(string? stockSymbol);
    }
}
