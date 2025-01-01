﻿using APIProductCatalog.Context;
using APIProductCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        const string notFoundMessage = "Product Not Found.";
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            _logger.LogInformation("=========== GET api/products/ ===========");

            var products = await _context.Products.AsNoTracking().ToListAsync();
            if (products is null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            _logger.LogInformation($"=========== GET api/products/id = {id} ===========");
            
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product is null)
            {
                _logger.LogInformation($"=========== GET api/products/id = {id} NOT FOUND ===========");
                return NotFound(notFoundMessage);
            }
            return Ok(product);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (product is null)
                return BadRequest();

            _context.Products.Add(product);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetProduct",
                new { id = product.ProductId }, product);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product is null)
                return NotFound(notFoundMessage);

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok();
        }
    }
}
