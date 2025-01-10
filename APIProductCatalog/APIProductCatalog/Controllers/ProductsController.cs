using APIProductCatalog.DTOs;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using APIProductCatalog.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIProductCatalog.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ProductsController : ControllerBase
    {
        const string notFoundMessage = "Product Not Found.";
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public ProductsController(IUnitOfWork uow, ILogger<ProductsController> logger, IMapper mapper)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("category/{id}")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsByCategory(int id)
        {
            var products = _uow.ProductRepository.GetProductsByCategory(id);
            if (products is null)
                return NotFound();

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = _uow.ProductRepository.GetProducts(productsParameters);

            return GetProducts(products);
        }

        [HttpGet("filter/price/pagination")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsByPrice([FromQuery] ProductsFilterPrice productsFilterPrice)
        {
            var products = _uow.ProductRepository.GetProductsByPrice(productsFilterPrice);
            return GetProducts(products);
        }
    
        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> Get()
        {
            _logger.LogInformation("=========== GET api/products/ ===========");

            var products = _uow.ProductRepository.GetAll();
            if (products is null)
                return NotFound();

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public ActionResult<ProductDTO> Get(int id)
        {
            _logger.LogInformation($"=========== GET api/products/id = {id} ===========");

            var product = _uow.ProductRepository.Get(p => p.ProductId == id);
            if (product is null)
            {
                _logger.LogInformation($"=========== GET api/products/id = {id} NOT FOUND ===========");
                return NotFound(notFoundMessage);
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public ActionResult<ProductDTO> Post([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var createdProduct = _uow.ProductRepository.Create(product);
            _uow.Commit();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);

            return new CreatedAtRouteResult("GetProduct",
                new { id = createdProductDto.ProductId }, createdProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public ActionResult<ProductDTOUpdateResponse> Patch(int id, [FromBody] JsonPatchDocument<ProductDTOUpdateRequest> patchProductDto)
        {
            if (patchProductDto is null || id <= 0)
                return BadRequest();

            var product = _uow.ProductRepository.Get(c => c.ProductId == id);

            var productUpdateDtoRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDto.ApplyTo(productUpdateDtoRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(productUpdateDtoRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateDtoRequest, product);

            _uow.ProductRepository.Update(product);
            _uow.Commit();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult<ProductDTO> Put(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.ProductId)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uow.ProductRepository.Update(product);
            _uow.Commit();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult<ProductDTO> Delete(int id)
        {
            var product = _uow.ProductRepository.Get(p => p.ProductId == id);

            if (product is null)
                return NotFound("Product not found.");
            
            var deletedProduct = _uow.ProductRepository.Delete(product);
            _uow.Commit();

            var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDto);
        }
        private ActionResult<IEnumerable<ProductDTO>> GetProducts(PagedList<Product> products)
        {
            var metadata = new
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }
    }
}
