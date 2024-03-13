﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;

        public StocksService(bool initialize = true)
        {
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();

            if (initialize)
            {
                _buyOrders.AddRange(new List<BuyOrder>()
                {
                    
                });
            }
        }

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            throw new NotImplementedException();
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            throw new NotImplementedException();
        }
    }
}
