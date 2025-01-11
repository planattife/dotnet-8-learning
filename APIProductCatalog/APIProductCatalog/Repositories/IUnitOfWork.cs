namespace APIProductCatalog.Repositories;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }

    Task CommitAsync();
}
