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
    public ActionResult<IEnumerable<Category>> Get()
    {
        var categories = _uow.CategoryRepository.GetAll();
        return Ok(categories);
    }

    [HttpGet("{id:int:min(1)}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _uow.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
            return BadRequest("Invalid Data.");

        var createdCategory = _uow.CategoryRepository.Create(category);
        _uow.Commit();

        return new CreatedAtRouteResult("GetCategory",
            new { id = createdCategory.CategoryId }, createdCategory);
    }

    [HttpPut("{id:int:min(1)}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest("Invalid Data.");

        var updatedCategory = _uow.CategoryRepository.Update(category);
        _uow.Commit();

        return Ok(updatedCategory);
    }

    [HttpDelete("{id:int:min(1)}")]
    public ActionResult Delete(int id)
    {
        var category = _uow.CategoryRepository.Get(c => c.CategoryId == id);

        if (category is null)
            return NotFound(notFoundMessage);

        var deletedCategory = _uow.CategoryRepository.Delete(category);
        _uow.Commit();

        return Ok(deletedCategory);
    }
}
