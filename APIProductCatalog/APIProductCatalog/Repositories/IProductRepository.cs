using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using X.PagedList;

namespace APIProductCatalog.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productParams);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
    Task<IPagedList<Product>> GetProductsByPriceAsync(ProductsFilterPrice productFilterParams);
}
