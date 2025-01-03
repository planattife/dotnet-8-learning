using APIProductCatalog.Context;
using APIProductCatalog.Models;
using APIProductCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        const string notFoundMessage = "Product Not Found.";
        private readonly IProductRepository _repository;
        private readonly ILogger _logger;
        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            _logger.LogInformation("=========== GET api/products/ ===========");

            var products = _repository.GetProducts().ToList();
            if (products is null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            _logger.LogInformation($"=========== GET api/products/id = {id} ===========");

            var product = _repository.GetProduct(id);
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

            var createdProduct = _repository.Create(product);

            return new CreatedAtRouteResult("GetProduct",
                new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            bool updated = _repository.Update(product);

            if (updated)
                return Ok(product);
            else
                return StatusCode(500, $"There was an error when trying to update product with id = {id}");
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            bool deleted = _repository.Delete(id);

            if (deleted)
                return Ok("Product deleted.");
            else
                return StatusCode(500, $"There was an error when trying to delete product with id = {id}");
        }
    }
}
