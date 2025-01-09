using APIProductCatalog.Context;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;

namespace APIProductCatalog.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{

    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Product> GetProducts(ProductsParameters productParams)
    {
        var products = GetAll()
            .OrderBy(p => p.ProductId)
            .AsQueryable();

        var paginatedProducts = PagedList<Product>.ToPagedList(products, productParams.PageNumber, productParams.PageSize);
        return paginatedProducts;
    }

    public IEnumerable<Product> GetProductsByCategory(int id)
    {
        return GetAll().Where(c => c.CategoryId == id);
    }
}
