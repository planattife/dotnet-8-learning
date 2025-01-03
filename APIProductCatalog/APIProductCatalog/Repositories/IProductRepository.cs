using APIProductCatalog.Models;

namespace APIProductCatalog.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategory(int id);
}
