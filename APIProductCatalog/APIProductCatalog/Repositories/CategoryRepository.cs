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

    public PagedList<Category> GetCategoriesByName(CategoriesFilterName categoriesFilterName)
    {
        var categories = GetAll()
            .AsQueryable();

        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
        }

        var paginatedCategories = PagedList<Category>.ToPagedList(categories, categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
        return paginatedCategories;
    }
}
