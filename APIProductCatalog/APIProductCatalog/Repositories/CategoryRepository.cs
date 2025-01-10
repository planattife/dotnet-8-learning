using APIProductCatalog.Context;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Category> GetCategories(CategoriesParameters categoriesParams)
    {
        var categories = GetAll()
            .OrderBy(c => c.CategoryId)
            .AsQueryable();

        var paginatedCategories = PagedList<Category>.ToPagedList(categories, categoriesParams.PageNumber, categoriesParams.PageSize);
        return paginatedCategories;
    }
}
