namespace RepositoryContracts
{
    namespace ServiceContracts
    {
        /// <summary>
        /// Represents the data-access logic for managing data from Finnhub
        /// </summary>
        public interface IFinnhubRespository
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

            /// <summary>
            /// Returns a list of supported stocks 
            /// </summary>
            /// <returns>A list of dictionaries with supported exchange codes. The Response Attributes are listed below:
            ///
            /// currency
            /// Price's currency. This might be different from the reporting currency of fundamental data.
            /// 
            /// description
            /// Symbol description
            /// 
            /// displaySymbol
            /// Display symbol name.
            /// 
            /// figi
            /// FIGI identifier.
            /// 
            /// isin
            /// ISIN. This field is only available for EU stocks and selected Asian markets. Entitlement from Finnhub is required to access this field.
            /// 
            /// mic
            /// Primary exchange's MIC.
            /// 
            /// shareClassFIGI
            /// Global Share Class FIGI.
            /// 
            /// symbol
            /// Unique symbol used to identify this symbol used in /stock/candle endpoint.
            /// 
            /// symbol2
            /// Alternative ticker for exchanges with multiple tickers for 1 stock such as BSE.
            /// 
            /// type
            /// Security type.
            ///
            /// </returns>
            Task<List<Dictionary<string, string>>?> GetStocks();

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

}
