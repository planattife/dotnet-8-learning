using APIProductCatalog.Models;
using APIProductCatalog.Pagination;

namespace APIProductCatalog.Repositories;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetProducts(ProductsParameters productParams);
    IEnumerable<Product> GetProductsByCategory(int id);
    PagedList<Product> GetProductsByPrice(ProductsFilterPrice productFilterParams);
}
