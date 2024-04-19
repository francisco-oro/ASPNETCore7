using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ServiceContracts;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StockAppTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubStocksService _finnhubStocksService; 
        private readonly IFinnhubSearchStocksService _finnhubSearchStocksService;

        private readonly Mock<IFinnhubStocksService> _finnhubStocksServiceMock;
        private readonly Mock<IFinnhubSearchStocksService> _finnhubSearchStocksServiceMock;

        private readonly ILogger<StocksController> _logger;

        private readonly Fixture _fixture;
        private readonly IOptions<TradingOptions> _options;
        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubCompanyProfileService>();
            _finnhubService = _finnhubServiceMock.Object;

            _options = Options.Create(new TradingOptions()
            {
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO",
                DefaultStockSymbol = "US"
            });
            _logger = new Mock<ILogger<StocksController>>().Object;
        }

        #region Explore

        [Fact]
        public async Task Explore_IfNullStockParameter_ToExploreView()
        {
            //Arrange
            string stockSymbolsJson = File.ReadAllText($"C:\\Users\\ASUS\\source\\repos\\ASPNETCore7\\ASPNETCore7\\src\\UnitTesting - Advanced - Moq\\StocksAppAssignment\\StockAppTests\\stockSymbols.json");

            List<Dictionary<string, string>>? stocksDictionary =
                JsonSerializer.Deserialize<List<Dictionary<string, string>>?>(stockSymbolsJson);
 
            _finnhubServiceMock.Setup(
                    temp => temp.GetStocks())
                .ReturnsAsync(stocksDictionary);

            StocksController stocksController = new StocksController(_finnhubService, _options, _logger);

            //Act
            IActionResult result = await stocksController.Explore(searchBy: null, stock: null, showAll: false);
            //Assert 
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<Stock>>();
        }

        #endregion
    }
}
