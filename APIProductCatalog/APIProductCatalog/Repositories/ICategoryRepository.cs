using APIProductCatalog.Models;
using APIProductCatalog.Pagination;

namespace APIProductCatalog.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    PagedList<Category> GetCategories(CategoriesParameters categoriesParams);
    PagedList<Category> GetCategoriesByName(CategoriesFilterName categoriesFilterName);

}
