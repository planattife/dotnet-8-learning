namespace APIProductCatalog.Pagination;

public class ProductsFilterPrice : QueryStringParameters
{
    public decimal? Price { get; set; }
    public string? PriceCriteria { get; set; }
}
