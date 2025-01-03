using APIProductCatalog.Context;
using APIProductCatalog.Models;

namespace APIProductCatalog.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Product> GetProducts()
    {
        return _context.Products;

    }

    public Product GetProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
            throw new InvalidOperationException("Product not found");
        return product;
    }

    public Product Create(Product product)
    {
        if (product is null)
            throw new InvalidOperationException("Invalid Data.");

        _context.Products.Add(product);
        _context.SaveChanges();
        return product;
    }

    public bool Update(Product product)
    {
        if (product is null)
            throw new InvalidOperationException("Invalid Data.");

        if (_context.Products.Any(p => p.ProductId == product.ProductId))
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product is not null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}
