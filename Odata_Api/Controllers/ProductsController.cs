﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Odata_Api.Models;

namespace Odata_Api.Controllers
{

    public class ProductsController : ODataController
    {
        private readonly MyStoreContext _context;

        public ProductsController(MyStoreContext context)
        {
            _context = context;
        }

        // GET: odata/Products
        [EnableQuery]
        public IActionResult Get()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return Ok(_context.Products.Include(p => p.Category));
        }

        // PUT: odata/Products({key})
        [HttpPut("{key}")]
        [EnableQuery]
        public async Task<IActionResult> Put(int key, [FromBody] Product product)
        {
            if (key != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
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

        // POST: odata/Products
        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MyStoreContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Created(product);
        }

        // DELETE: odata/Products({key})
        [HttpDelete("{key}")]
        [EnableQuery]
        public async Task<IActionResult> Delete(int key)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int key)
        {
            return (_context.Products?.Any(e => e.ProductId == key)).GetValueOrDefault();
        }
    }
}
