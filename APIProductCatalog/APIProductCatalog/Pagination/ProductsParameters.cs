namespace APIProductCatalog.Pagination;

public class ProductsParameters
{
    const int maxPageSize = int.MaxValue;
    public int PageNumber { get; set; } = 1;
    private int _pageSize;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
