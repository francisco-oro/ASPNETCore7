using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{

    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _context;

        public StocksRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            await _context.BuyOrders.AddAsync(buyOrder);
            await _context.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            await _context.SellOrders.AddAsync(sellOrder);
            await _context.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _context.BuyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _context.SellOrders.ToListAsync();
        }
    }
}
