using APIProductCatalog.DTOs;
using APIProductCatalog.DTOs.Mappings;
using APIProductCatalog.Filters;
using APIProductCatalog.Models;
using APIProductCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;

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
    public ActionResult<IEnumerable<CategoryDTO>> Get()
    {
        var categories = _uow.CategoryRepository.GetAll();

        var categoriesDto = categories.ToCategoryDTOList();

        return Ok(categoriesDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public ActionResult<CategoryDTO> Get(int id)
    {
        var category = _uow.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        var categoryDto = category.ToCategoryDTO();

        return Ok(categoryDto);
    }

    [HttpPost]
    public ActionResult<CategoryDTO> Post(CategoryDTO categoryDto)
    {
        if (categoryDto is null)
            return BadRequest("Invalid Data.");

        var category = categoryDto.ToCategory();

        var createdCategory = _uow.CategoryRepository.Create(category);
        _uow.Commit();

        var createdCategoryDto = createdCategory.ToCategoryDTO();

        return new CreatedAtRouteResult("GetCategory",
            new { id = createdCategoryDto.CategoryId }, createdCategoryDto);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult<CategoryDTO> Put(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
            return BadRequest("Invalid Data.");

        var category = categoryDto.ToCategory();

        var updatedCategory = _uow.CategoryRepository.Update(category);
        _uow.Commit();

        var updatedCategoryDto = updatedCategory.ToCategoryDTO();

        return Ok(updatedCategoryDto);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult<CategoryDTO> Delete(int id)
    {
        var category = _uow.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        var deletedCategory = _uow.CategoryRepository.Delete(category);
        _uow.Commit();

        var deletedCategoryDto = deletedCategory.ToCategoryDTO();

        return Ok(deletedCategoryDto);
    }
}
