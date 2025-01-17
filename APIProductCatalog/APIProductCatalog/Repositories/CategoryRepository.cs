﻿using APIProductCatalog.Context;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using X.PagedList;

namespace APIProductCatalog.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
    {
        var categories = await GetAllAsync();
        var sortedCategories = categories.OrderBy(c => c.CategoryId).AsQueryable();

        var result = await sortedCategories.ToPagedListAsync(categoriesParams.PageNumber, categoriesParams.PageSize);
        return result;
    }

    public async Task<IPagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName)
    {
        var categories = await GetAllAsync();

        if (!string.IsNullOrEmpty(categoriesFilterName.Name))
        {
            categories = categories.Where(c => c.Name.Contains(categoriesFilterName.Name));
        }

        var paginatedCategories = await categories.ToPagedListAsync(categoriesFilterName.PageNumber, categoriesFilterName.PageSize);
        return paginatedCategories;
    }
}
