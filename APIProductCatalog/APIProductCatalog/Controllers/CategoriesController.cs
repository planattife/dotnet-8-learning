using APIProductCatalog.Context;
using APIProductCatalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    const string notFoundMessage = "Category Not Found.";

    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesProducts()
    {
        return _context.Categories
            .Include(p => p.Products)
            .ToList();
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        return _context.Categories.ToList();
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public ActionResult<Category> Get(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category is null)
            return NotFound(notFoundMessage);
        return Ok(category);
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category is null)
            return BadRequest();

        _context.Categories.Add(category);
        _context.SaveChanges();

        return new CreatedAtRouteResult("GetCategory",
            new { id = category.CategoryId }, category);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest();

        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category is null)
            return NotFound(notFoundMessage);

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return Ok();
    }
}
