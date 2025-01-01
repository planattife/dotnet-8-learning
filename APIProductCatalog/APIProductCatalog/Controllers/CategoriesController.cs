using APIProductCatalog.Context;
using APIProductCatalog.Filters;
using APIProductCatalog.Models;
using APIProductCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    const string notFoundMessage = "Category Not Found.";

    private readonly ICategoryRepository _repository;

    public CategoriesController(ICategoryRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _repository.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _repository.GetCategory(id);

        if (category is null)
            return NotFound(notFoundMessage);

        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
            return BadRequest("Invalid Data.");

        var createdCategory = _repository.Create(category);

        return new CreatedAtRouteResult("GetCategory",
            new { id = createdCategory.CategoryId }, createdCategory);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest("Invalid Data.");

        var updatedCategory = _repository.Update(category);

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _repository.GetCategory(id);

        if (category is null)
            return NotFound(notFoundMessage);

        var deletedCategory = _repository.Delete(id);

        return Ok(deletedCategory);
    }
}
