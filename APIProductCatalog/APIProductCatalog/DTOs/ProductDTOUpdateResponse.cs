namespace APIProductCatalog.DTOs;

public class ProductDTOUpdateResponse
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }
}
