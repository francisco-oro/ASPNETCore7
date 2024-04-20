using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace StockAppTests
{
     public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory applicationFactory)
        {
            _httpClient = applicationFactory.CreateClient();    
        }

        #region Index

        [Fact]
        public async Task Index_ToViewResult()
        {
            // Arrange
            string stockSymbol = "MSFT";
            // Act 
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"Trade/Index/{stockSymbol}");

            // Assert
            httpResponseMessage.Should().BeSuccessful(); //2xx
            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml( responseBody );
            var document = html.DocumentNode;

            document.QuerySelector(".price").Should().NotBeNull();
        }

        #endregion
    }
}
