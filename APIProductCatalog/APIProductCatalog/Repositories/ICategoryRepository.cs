using APIProductCatalog.Models;
using APIProductCatalog.Pagination;

namespace APIProductCatalog.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<PagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams);
    Task<PagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName);
}
