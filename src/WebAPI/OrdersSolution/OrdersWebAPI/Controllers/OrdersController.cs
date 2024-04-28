using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersWebAPI.DbContext;
using OrdersWebAPI.Entities;

namespace OrdersWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : CustomControllerBase
    {
        private int _orderCount;
        public OrdersController(ApplicationDbContext context) : base(context)
        {
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return await _context.Order.Include(order1 => order1.OrderItems).ToListAsync();

        }
        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _context.Order.Include(o => o.OrderItems).FirstOrDefaultAsync(temp => temp.OrderId == id);

            if (order == null)
            {
                return Problem(detail:"Invalid id", statusCode: 404, title: "Order Search");
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            var existingOrder = await _context.Order.Include(order1 => order1.OrderItems).FirstOrDefaultAsync(temp => temp.OrderId == id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.CustomerName = order.CustomerName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            order.OrderNumber = $"Order_{DateTime.Today.Year}_{GetInteger()}";
            order.OrderDate = DateTime.Now;
            if (order.OrderItems != null)
            {                
                order.TotalAmount = order.OrderItems.Sum(temp => temp.TotalPrice);
            }

            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _context.Order.Include(order1 => order1.OrderItems).FirstOrDefaultAsync(temp => temp.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(Guid id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        private int GetInteger()
        {
            return _orderCount++;
        }
    }
}
