namespace ServiceContracts
{
    /// <summary>
    /// Represents the business logic for retrieving data from Finnhub
    /// </summary>
    public interface IFinnhubSearchStocksService
    {
        /// <summary>
        /// Search for best-matching symbols based on a query. It accepts any input from symbol, security's name to ISIN and Cusip
        /// </summary>
        /// <param name="query">Query text to search. It can be symbol, name, ISIn or CUSIP</param>
        /// <returns>A dictionary with information about the best-matching symbols. Response attributes:
        ///
        /// count
        /// Number of results.
        /// 
        /// result
        /// Array of search results.
        /// 
        /// description
        /// Symbol description
        /// 
        /// displaySymbol
        /// Display symbol name.
        /// 
        /// symbol
        /// Unique symbol used to identify this symbol used in /stock/candle endpoint.
        /// 
        /// type
        /// Security type.
        /// </returns>
        Task<Dictionary<string, object>?> SearchStocks(string query);
    }
}
