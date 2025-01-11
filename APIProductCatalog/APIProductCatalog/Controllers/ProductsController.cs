using APIProductCatalog.DTOs;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using APIProductCatalog.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int id)
        {
            var products = await _uow.ProductRepository.GetProductsByCategoryAsync(id);
            if (products is null)
                return NotFound();

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _uow.ProductRepository.GetProductsAsync(productsParameters);

            return GetProducts(products);
        }

        [HttpGet("filter/price/pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByPrice([FromQuery] ProductsFilterPrice productsFilterPrice)
        {
            var products = await _uow.ProductRepository.GetProductsByPriceAsync(productsFilterPrice);
            return GetProducts(products);
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            _logger.LogInformation("=========== GET api/products/ ===========");

            var products = await _uow.ProductRepository.GetAllAsync();
            if (products is null)
                return NotFound();

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDto);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            _logger.LogInformation($"=========== GET api/products/id = {id} ===========");

            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);
            if (product is null)
            {
                _logger.LogInformation($"=========== GET api/products/id = {id} NOT FOUND ===========");
                return NotFound(notFoundMessage);
            }

            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (productDto is null)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var createdProduct = _uow.ProductRepository.Create(product);
            await _uow.CommitAsync();

            var createdProductDto = _mapper.Map<ProductDTO>(createdProduct);

            return new CreatedAtRouteResult("GetProduct",
                new { id = createdProductDto.ProductId }, createdProductDto);
        }

        [HttpPatch("{id}/UpdatePartial")]
        public async Task<ActionResult<ProductDTOUpdateResponse>> Patch(int id, [FromBody] JsonPatchDocument<ProductDTOUpdateRequest> patchProductDto)
        {
            if (patchProductDto is null || id <= 0)
                return BadRequest();

            var product = await _uow.ProductRepository.GetAsync(c => c.ProductId == id);

            var productUpdateDtoRequest = _mapper.Map<ProductDTOUpdateRequest>(product);

            patchProductDto.ApplyTo(productUpdateDtoRequest, ModelState);

            if (!ModelState.IsValid || TryValidateModel(productUpdateDtoRequest))
                return BadRequest(ModelState);

            _mapper.Map(productUpdateDtoRequest, product);

            _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            return Ok(_mapper.Map<ProductDTOUpdateResponse>(product));
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult<ProductDTO>> Put(int id, [FromBody] ProductDTO productDto)
        {
            if (id != productDto.ProductId)
                return BadRequest();

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            var updatedProductDto = _mapper.Map<ProductDTO>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _uow.ProductRepository.GetAsync(p => p.ProductId == id);

            if (product is null)
                return NotFound("Product not found.");
            
            var deletedProduct = _uow.ProductRepository.Delete(product);
            await _uow.CommitAsync();

            var deletedProductDto = _mapper.Map<ProductDTO>(deletedProduct);

            return Ok(deletedProductDto);
        }
        private ActionResult<IEnumerable<ProductDTO>> GetProducts(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.Count,
                products.PageSize,
                products.PageCount,
                products.TotalItemCount,
                products.HasNextPage,
                products.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDto);
        }
    }
}
