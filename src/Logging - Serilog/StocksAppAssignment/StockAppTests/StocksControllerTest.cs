using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using StocksApp;
using StocksApp.Controllers;
using StocksApp.Models;

namespace StockAppTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubService _finnhubService; 

        private readonly Mock<IFinnhubService> _finnhubServiceMock;

        private readonly Fixture _fixture;
        private readonly IOptions<TradingOptions> _options;
        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;

            _options = Options.Create(new TradingOptions()
            {
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO",
                DefaultStockSymbol = "US"
            });

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

            StocksController stocksController = new StocksController(_finnhubService, _options);

            //Act
            IActionResult result = await stocksController.Explore(searchBy: null, stock: null, showAll: false);
            //Assert 
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<List<Stock>>();
        }

        #endregion
    }
}
