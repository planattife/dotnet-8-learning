using APIProductCatalog.Context;
using APIProductCatalog.Models;
using APIProductCatalog.Pagination;
using X.PagedList;

namespace APIProductCatalog.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{

    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productParams)
    {
        var products = await GetAllAsync();
        var sortedProducts = products
            .OrderBy(p => p.ProductId)
            .AsQueryable();

        var result = await sortedProducts.ToPagedListAsync(productParams.PageNumber, productParams.PageSize);
        return result;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int id)
    {
        var products = await GetAllAsync();
        var filteredProducts = products.Where(c => c.CategoryId == id);
        return filteredProducts;
    }

    public async Task<IPagedList<Product>> GetProductsByPriceAsync(ProductsFilterPrice productFilterParams)
    {
        var products = await GetAllAsync();
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

        var paginatedProducts = await products.ToPagedListAsync(productFilterParams.PageNumber, productFilterParams.PageSize);
        return paginatedProducts;
    }
}
