using APIProductCatalog.Models;
using APIProductCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APIProductCatalog.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        const string notFoundMessage = "Product Not Found.";
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;

        public ProductsController(IUnitOfWork uow, ILogger<ProductsController> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        [HttpGet("category/{id}")]
        public ActionResult<IEnumerable<Product>> GetProductsByCategory(int id)
        {
            var products = _uow.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound();

            return Ok(products);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            _logger.LogInformation("=========== GET api/products/ ===========");

            var products = _uow.ProductRepository.GetAll();
            if (products is null)
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public ActionResult<Product> Get(int id)
        {
            _logger.LogInformation($"=========== GET api/products/id = {id} ===========");

            var product = _uow.ProductRepository.Get(p => p.ProductId == id);
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

            var createdProduct = _uow.ProductRepository.Create(product);
            _uow.Commit();

            return new CreatedAtRouteResult("GetProduct",
                new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            var updatedProduct = _uow.ProductRepository.Update(product);
            _uow.Commit();

            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            var product = _uow.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
                return NotFound("Product not found.");
            
            var deletedProduct = _uow.ProductRepository.Delete(product);
            _uow.Commit();

            return Ok(deletedProduct);
        }
    }
}
