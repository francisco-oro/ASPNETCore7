using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using RepositoryContracts.ServiceContracts;
using Microsoft.Extensions.Configuration;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRespository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object?>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri =
                        new Uri(
                            $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Forbidden))
                {
                    throw new AuthenticationException("Not authorized to see this resource");
                }

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader reader = new StreamReader(stream);

                string response = reader.ReadToEnd();
                Dictionary<string, object?>? responseDictionary
                    = JsonSerializer.Deserialize<Dictionary<string, object>>(response);
                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from finnhub server");
                }

                if (responseDictionary.ContainsKey("error"))
                {
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                }
                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri =
                        new Uri(
                            $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Forbidden))
                {
                    throw new AuthenticationException("Not authorized to see this resource");
                }

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader reader = new StreamReader(stream);

                string response = reader.ReadToEnd();

                Dictionary<string, object>? responseDictionary =
                    JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from Finnnhub server");
                }

                if (responseDictionary.TryGetValue("error", out var value))
                {
                    throw new InvalidOperationException(Convert.ToString(value));
                }
                return responseDictionary;
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            string exchangeSymbol = _configuration["TradingOptions:DefaultExchangeSymbol"] ?? "US";
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri =
                        new Uri(
                            $"https://finnhub.io/api/v1/stock/symbol?exchange={exchangeSymbol}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                string response = reader.ReadToEnd();
                
                List<Dictionary<string, string>>? responseList =
                    JsonSerializer.Deserialize<List<Dictionary<string, string>?>>(response);

                if (responseList == null)
                {
                    throw new InvalidOperationException("No response from Finnhub server");
                }

                return responseList;
            }
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string query)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri =
                        new Uri(
                            $"https://finnhub.io/api/v1/search?q={query}&token={_configuration["FinnhubToken"]}"),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader reader = new StreamReader(stream);

                string response = reader.ReadToEnd();

                Dictionary<string, object>? responseDictionary =
                    JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                if (responseDictionary == null)
                {
                    throw new InvalidOperationException("No response from Finnnhub server");
                }

                if (responseDictionary.TryGetValue("error", out var value))
                {
                    throw new InvalidOperationException(Convert.ToString(value));
                }
                return responseDictionary;
            }
        }
    }

}
