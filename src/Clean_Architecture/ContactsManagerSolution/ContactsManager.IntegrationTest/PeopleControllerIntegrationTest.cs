using FluentAssertions;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace ContactsManager.IntegrationTests
{
    public class PeopleControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public PeopleControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Index

        [Fact]
        public async Task Index_ToReturnView()
        {
            //Arrange

            //Act
            HttpResponseMessage response = await _client.GetAsync("/People/Index"); 

            //Assert
            response.Should().BeSuccessful(); //2xx

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll("table.people").Should().NotBeNull();
        }

        #endregion
    } 
}
