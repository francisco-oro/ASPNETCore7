using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace StockAppTests
{
    public class StocksServiceTest
    {
        // private fields
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;

        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly ILogger<StocksService> _loggerMock;
        private readonly IStocksRepository _stocksRepository;

        private readonly IFixture _fixture;
        //constructor
        public StocksServiceTest(ITestOutputHelper outputHelper)
        {
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            var sellOrdersInitialData = new List<SellOrder>() { };
            if (sellOrdersInitialData == null) throw new ArgumentNullException(nameof(sellOrdersInitialData));
            var buyOrdersInitialData  = new List<BuyOrder>() { };
            if (buyOrdersInitialData == null) throw new ArgumentNullException(nameof(buyOrdersInitialData));

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext  dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.BuyOrders, buyOrdersInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.SellOrders, sellOrdersInitialData);
            _loggerMock = new Mock<ILogger<StocksService>>().Object;
            _stocksService = new StocksService(_stocksRepository, _loggerMock);

            _outputHelper = outputHelper;
        }

        // helper methods



        #region CreateBuyOrder
        // 1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;


            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BuyOrderQuantityIsLowerThanMinimumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_BuyOrderQuantityIsGreaterThanMaximumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 100001,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_OrderPriceIsLowerThanMinimumAllowed()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 0,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_OrderPriceIsGreaterThanMaximumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 10001,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_DateAndTimeOfOrderIsOlderThanMinimumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_ValidInputValues()
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Create<BuyOrderRequest>();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            _stocksRepositoryMock.Setup(
                    temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            buyOrderResponseExpected.BuyOrderID = buyOrderResponse.BuyOrderID;

            buyOrderResponse.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponse.Should().Be(buyOrderResponseExpected);
        }
        #endregion

        #region CreateSellOrder

        // 1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrder()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;


            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderQuantityIsLowerThanMinimumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderQuantityIsGreaterThanMaximumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 100001,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderPriceIsLowerThanMinimumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 0,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_SellOrderPriceISGreaterThanMaximumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 10001,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_DateAndTimeOlderThanMinimumDate()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidSellOrder()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Create<SellOrderRequest>();
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            SellOrderResponse sellOrderResponseExpected = sellOrder.ToSellOrderResponse();

            _stocksRepositoryMock.Setup(
                    temp => temp.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            sellOrderResponseExpected.SellOrderID = sellOrderResponse.SellOrderID;

            sellOrderResponse.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponse.Should().Be(sellOrderResponseExpected);
        }

        #endregion

        #region GetAllBuyOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_EmptyList()
        {
            //Arrange
            _stocksRepositoryMock.Setup(
                    temp => temp.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());
            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponsesFromGet);
        }

        // 2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders()
        {
            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
                _fixture.Create<BuyOrder>(),
                _fixture.Create<BuyOrder>()
            };

            _stocksRepositoryMock.Setup(
                    temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);
            //Act

            List<BuyOrderResponse> buyOrderResponsesExpected  = buyOrders.Select(temp => temp.ToBuyOrderResponse()).ToList();

            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetBuyOrders();

            buyOrderResponsesFromGet.Should().BeEquivalentTo(buyOrderResponsesFromGet);
        }
        #endregion

        #region GetAllSellOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_EmptyList()
        {
            _stocksRepositoryMock.Setup(
                    temp => temp.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());
            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponsesFromGet);
        }

        // 2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders()
        {
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
                _fixture.Create<SellOrder>(),
                _fixture.Create<SellOrder>()
            };
            _stocksRepositoryMock.Setup(
                    temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);
            List<SellOrderResponse> sellOrderResponsesExpected =
                sellOrders.Select(temp => temp.ToSellOrderResponse()).ToList();

            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetSellOrders();

            sellOrderResponsesFromGet.Should().BeEquivalentTo(sellOrderResponsesExpected);
        }
        #endregion
    }
}