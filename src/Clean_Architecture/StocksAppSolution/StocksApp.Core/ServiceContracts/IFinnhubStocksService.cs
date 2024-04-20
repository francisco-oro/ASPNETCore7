namespace StocksApp.Core.ServiceContracts
{
    /// <summary>
    /// Represents the business logic for retrieving data from Finnhub
    /// </summary>
    public interface IFinnhubStocksService
    {
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
    }
}
