using APIProductCatalog.Context;
using APIProductCatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIProductCatalog.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{

    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}
