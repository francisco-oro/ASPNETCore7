using ServiceContracts;
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
            _stocksService = new StocksService();
        }

        // helper methods



        #region CreateBuyOrder
        // When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrder()
        {

        }

        // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIsLowerThanMinimumValue()
        {

        }

        // When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantityIsGreaterThanMaximumValue()
        {

        }

        // When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrder()
        {

        }

        // When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrder()
        {

        }
        #endregion


    }
}