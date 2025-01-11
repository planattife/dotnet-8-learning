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

    public async Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
    {
        var categories = await GetAllAsync();
        var sortedCategories = categories.OrderBy(c => c.CategoryId).AsQueryable();

        var result = PagedList<Category>.ToPagedList(sortedCategories, categoriesParams.PageNumber, categoriesParams.PageSize);
        return result;
    }

    public async Task<PagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName)
    {
        var categories = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
        }

        var paginatedCategories = PagedList<Category>.ToPagedList(categories.AsQueryable(), categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
        return paginatedCategories;
    }
}
