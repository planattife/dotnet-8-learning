using APIProductCatalog.Validations;
using System.ComponentModel.DataAnnotations;

namespace APIProductCatalog.DTOs;

public class CategoryDTO
{
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [StringLength(80)]
    [StartWithUpperCaseAttribute]
    public string? Name { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
}
