using APIProductCatalog.DTOs;
using APIProductCatalog.DTOs.Mappings;
using APIProductCatalog.Filters;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using APIProductCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIProductCatalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    const string notFoundMessage = "Category Not Found.";

    private readonly IUnitOfWork _uow;

    public CategoriesController(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
    {
        var categories = await _uow.CategoryRepository.GetAllAsync();

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
    {
        var categories = await _uow.CategoryRepository.GetCategoriesAsync(categoriesParameters);

        return GetCategories(categories);
    }

    [HttpGet("filter/name/pagination")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesByName([FromQuery] CategoriesFilterName categoriesParameters)
    {
        var categories = await _uow.CategoryRepository.GetCategoriesByNameAsync(categoriesParameters);
        return GetCategories(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var category = await _uow.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        var categoryDto = category.ToCategoryDTO();

        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
            return BadRequest("Invalid Data.");

        var category = categoryDto.ToCategory();

        var createdCategory = _uow.CategoryRepository.Create(category);
        await _uow.CommitAsync();

        var createdCategoryDto = createdCategory.ToCategoryDTO();

        return new CreatedAtRouteResult("GetCategory",
            new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
            return BadRequest("Invalid Data.");

        var category = categoryDto.ToCategory();

        var updatedCategory = _uow.CategoryRepository.Update(category);
        await _uow.CommitAsync();

        var updatedCategoryDto = updatedCategory.ToCategoryDTO();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<ActionResult<CategoryDTO>> Delete(int id)
    {
        var category = await _uow.CategoryRepository.GetAsync(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        var deletedCategory = _uow.CategoryRepository.Delete(category);
        await _uow.CommitAsync();

        var deletedCategoryDto = deletedCategory.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }

    private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
    {
        var metadata = new
        {
            categories.TotalCount,
            categories.PageSize,
            categories.CurrentPage,
            categories.TotalPages,
            categories.HasNext,
            categories.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriesDto = categories.ToCategoryDTOList();
        return Ok(categoriesDto);
    }
}
