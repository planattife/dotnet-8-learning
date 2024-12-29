using APIProductCatalog.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APIProductCatalog.Models;

[Table("Products")]
public class Product : IValidatableObject
{
    [Key]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(80, ErrorMessage = "Name must have between 5 and 80 characters.", MinimumLength = 4)]
    [StartWithUpperCaseAttribute]

    public string? Name { get; set; }

    [Required]
    [StringLength(300, ErrorMessage = "Description must have a maximum of {1} characters.")]
    public string? Description { get; set; }

    [Required]
    [Range(1, 1000000, ErrorMessage = "Price must be between {1} and {2}.")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (this.Quantity <= 0)
        {
            yield return new
                ValidationResult("Quantity must be greater than 0.",
                new[] { nameof(this.Quantity) });
        }
    }
}
