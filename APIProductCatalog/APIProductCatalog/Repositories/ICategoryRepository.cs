using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using X.PagedList;

namespace APIProductCatalog.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams);
    Task<IPagedList<Category>> GetCategoriesByNameAsync(CategoriesFilterName categoriesFilterName);
}
