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
        public ActionResult<IEnumerable<Product>> Get()
        {
            try
            {
                var products = _context.Products.AsNoTracking().ToList();
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

        [HttpGet("{id:int}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
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

        [HttpPut("{id:int}")]
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

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

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
