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

        //constructor
        public StocksServiceTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _stocksService = new StocksService(false);
        }

        // helper methods



        #region CreateBuyOrder
        // 1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrder()
        {
            // Arrange
            BuyOrderRequest? buyOrderRequest = null;


            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIsLowerThanMinimumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIsGreaterThanMaximumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 100001,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_OrderPriceIsLowerThanMinimumAllowed()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 0,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_OrderPriceIsGreaterThanMaximumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 10001,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = null
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_DateAndTimeOfOrderIsOlderThanMinimumValue()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidInputValues()
        {
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Microsoft",
                StockSymbol = "MSFT"
            };

            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();

            Assert.True(buyOrderResponse.BuyOrderID != Guid.Empty);
            Assert.Contains(buyOrderResponse, buyOrderResponses);
        }
        #endregion

        #region CreateSellOrder

        // 1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateSellOrder_NullSellOrder()
        {
            // Arrange
            SellOrderRequest? sellOrderRequest = null;


            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIsLowerThanMinimumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderQuantityIsGreaterThanMaximumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 100001,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceIsLowerThanMinimumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 0,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_SellOrderPriceISGreaterThanMaximumValue()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 10001,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = null
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 0,
                StockName = "Mirosoft",
                StockSymbol = null
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateSellOrder_DateAndTimeOlderThanMinimumDate()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // 8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidSellOrder()
        {
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

            Assert.True(sellOrderResponse.SellOrderID != Guid.Empty);
            Assert.Contains(sellOrderResponse, sellOrderResponses);
        }

        #endregion

        #region GetAllBuyOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_EmptyList()
        {
            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetBuyOrders();

            Assert.Empty(buyOrderResponsesFromGet);
        }

        // 2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders()
        {
            BuyOrderRequest? buyOrderRequest1 = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            BuyOrderRequest? buyOrderRequest2 = new BuyOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 32.1,
                Quantity = 8,
                StockName = "Apple",
                StockSymbol = "AAPL"
            };

            BuyOrderResponse buyOrderResponse1 = await _stocksService.CreateBuyOrder(buyOrderRequest1);
            BuyOrderResponse buyOrderResponse2 = await _stocksService.CreateBuyOrder(buyOrderRequest2);
            List<BuyOrderResponse> buyOrderResponsesFromAdd = new List<BuyOrderResponse>()
                { buyOrderResponse1, buyOrderResponse2 }; 

            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetBuyOrders();

            foreach (BuyOrderResponse buyOrderResponse in buyOrderResponsesFromAdd)
            {
                Assert.Contains(buyOrderResponse, buyOrderResponsesFromGet);
            }
        }
        #endregion

        #region GetAllSellOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_EmptyList()
        {
            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetSellOrders();

            Assert.Empty(sellOrderResponsesFromGet);
        }

        // 2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders()
        {
            SellOrderRequest? sellOrderRequest1 = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 23.4,
                Quantity = 4,
                StockName = "Mirosoft",
                StockSymbol = "MSFT"
            };

            SellOrderRequest? sellOrderRequest2 = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("2024-03-09"),
                Price = 32.1,
                Quantity = 8,
                StockName = "Apple",
                StockSymbol = "AAPL"
            };

            SellOrderResponse sellOrderResponse1 = await _stocksService.CreateSellOrder(sellOrderRequest1);
            SellOrderResponse sellOrderResponse2 = await _stocksService.CreateSellOrder(sellOrderRequest2);
            List<SellOrderResponse> sellOrderResponsesFromAdd = new List<SellOrderResponse>()
                { sellOrderResponse1, sellOrderResponse2 };

            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetSellOrders();

            foreach (SellOrderResponse sellOrderResponse in sellOrderResponsesFromAdd)
            {
                Assert.Contains(sellOrderResponse, sellOrderResponsesFromGet);
            }
        }
        #endregion
    }
}