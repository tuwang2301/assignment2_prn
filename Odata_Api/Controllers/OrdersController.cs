using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Odata_Api.Models;

namespace Odata_Api.Controllers
{
    public class OrdersController : ODataController
    {
        private readonly MyStoreContext _context;

        public OrdersController(MyStoreContext context)
        {
            _context = context;
        }

        // GET: odata/Orders
        [EnableQuery]
        public IActionResult Get()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return Ok(_context.Orders.Include(o => o.Staff));
        }


        // PUT: odata/Orders({key})
        [HttpPut("{key}")]
        [EnableQuery]
        public async Task<IActionResult> Put(int key, [FromBody] Order order)
        {
            if (key != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(key))
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

        // POST: odata/Orders
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'MyStoreContext.Orders'  is null.");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Created(order);
        }

        // DELETE: odata/Orders({key})
        [HttpDelete("{key}")]
        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(key);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int key)
        {
            return (_context.Orders?.Any(e => e.OrderId == key)).GetValueOrDefault();
        }

        // GET: odata/Orders/date
        [EnableQuery]
        [HttpGet("date")]
        public IActionResult GetOrdersByDate([FromQuery] DateTime orderDate)
        {
            var orders = _context.Orders.Where(o => o.OrderDate.Date == orderDate.Date);
            if (!orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }
    }
}
