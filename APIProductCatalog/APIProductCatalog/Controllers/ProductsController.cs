using APIProductCatalog.Context;
using APIProductCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        const string notFoundMessage = "Product Not Found.";
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            try
            {
                var products = await _context.Products.AsNoTracking().ToListAsync();
                if (products is null)
                    return NotFound();
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem while handling your request.");
            }          
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (product is null)
                    return NotFound(notFoundMessage);
                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem while handling your request.");
            }     
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            try
            {
                if (product is null)
                    return BadRequest();

                _context.Products.Add(product);
                _context.SaveChanges();

                return new CreatedAtRouteResult("GetProduct",
                    new { id = product.ProductId }, product);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem while handling your request.");
            }           
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                    return BadRequest();

                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem while handling your request.");
            }     
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product is null)
                    return NotFound(notFoundMessage);

                _context.Products.Remove(product);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was a problem while handling your request.");
            }    
        }
    }
}
