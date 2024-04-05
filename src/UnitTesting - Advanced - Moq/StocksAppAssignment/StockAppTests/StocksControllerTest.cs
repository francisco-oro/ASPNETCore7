using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using RepositoryContracts;
using ServiceContracts;

namespace StockAppTests
{
    internal class StocksControllerTest
    {
        private readonly IFinnhubService _finnhubService; 

        private readonly Mock<IFinnhubService> _finnhubServiceMock;

        private readonly Fixture _fixture;

        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _finnhubServiceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubServiceMock.Object;

        }
    }
}
