using APIProductCatalog.Models;
using APIProductCatalog.Pagination;

namespace APIProductCatalog.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> GetProductsAsync(ProductsParameters productParams);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id);
    Task<PagedList<Product>> GetProductsByPriceAsync(ProductsFilterPrice productFilterParams);
}
