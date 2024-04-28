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
    [Route("api/orders/{orderId}/items")]
    public class OrderItemsController : CustomControllerBase
    {
        public OrderItemsController(ApplicationDbContext context) : base(context)
        {
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(Guid orderId)
        {
            return await _context.OrderItem.Where(o => o.OrderId == orderId).ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(Guid orderId, Guid id)
        {
            var orderItem = await _context.OrderItem.Where(o => o.OrderId == orderId).FirstOrDefaultAsync(o => o.OrderItemId == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        // PUT: api/OrderItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(Guid orderId, Guid id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest();
            }

            var orderItemResult = await _context.OrderItem.FirstOrDefaultAsync(o => o.OrderItemId == id);
            if (orderItemResult is null)
            {
                return NotFound();
            }

            orderItemResult.UnitPrice = orderItem.UnitPrice;
            orderItemResult.Quantity = orderItem.Quantity;
            orderItemResult.ProductName = orderItem.ProductName;

            var orderResult = await _context.Order.FirstOrDefaultAsync(temp => temp.OrderId == orderId);
            orderResult.UpdateTotalAmount(); 

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
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

        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(Guid orderId, OrderItem orderItem)
        {
            if (orderId != orderItem.OrderId)
            {
                return BadRequest();
            }

            orderItem.OrderItemId = Guid.NewGuid();
            orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;

            _context.OrderItem.Add(orderItem);
            var existingOrder = await _context.Order.Include(o => o.OrderItems).FirstOrDefaultAsync(temp => temp.OrderId == orderId);
            existingOrder?.OrderItems?.Add(orderItem);
            existingOrder?.UpdateTotalAmount();
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderItem", new { id = orderItem.OrderItemId, orderId = orderId }, orderItem);
        }

        // DELETE: api/OrderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid orderId, Guid id)
        {
            var orderItem = await _context.OrderItem.FirstOrDefaultAsync(o => o.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItem.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(Guid id)
        {
            return _context.OrderItem.Any(e => e.OrderItemId == id);
        }
    }
}
