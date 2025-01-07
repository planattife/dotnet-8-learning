using APIProductCatalog.Validations;
using System.ComponentModel.DataAnnotations;

namespace APIProductCatalog.DTOs;

public class ProductDTO
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(80, ErrorMessage = "Name must have between 5 and 80 characters.", MinimumLength = 4)]
    [StartWithUpperCaseAttribute]
    public string? Name { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "Description must have a maximum of {1} characters.")]
    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public int CategoryId { get; set; }
}
