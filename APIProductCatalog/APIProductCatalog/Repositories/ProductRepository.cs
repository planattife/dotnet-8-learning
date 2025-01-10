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

    public PagedList<Product> GetProductsByPrice(ProductsFilterPrice productFilterParams)
    {
        var products = GetAll().AsQueryable();
        if (productFilterParams.Price.HasValue && !string.IsNullOrEmpty(productFilterParams.PriceCriteria))
        {
            if (productFilterParams.PriceCriteria.Equals("greater", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price > productFilterParams.Price.Value).OrderBy(p => p.Price);
            }
            if (productFilterParams.PriceCriteria.Equals("lower", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price < productFilterParams.Price.Value).OrderBy(p => p.Price);
            }
            if (productFilterParams.PriceCriteria.Equals("equal", StringComparison.OrdinalIgnoreCase))
            {
                products = products.Where(p => p.Price == productFilterParams.Price.Value).OrderBy(p => p.Price);
            }
        }

        var paginatedProducts = PagedList<Product>.ToPagedList(products, productFilterParams.PageNumber, productFilterParams.PageSize);
        return paginatedProducts;
    }
}
