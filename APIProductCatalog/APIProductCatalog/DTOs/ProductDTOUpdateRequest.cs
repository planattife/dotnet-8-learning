using System.ComponentModel.DataAnnotations;

namespace APIProductCatalog.DTOs;

public class ProductDTOUpdateRequest : IValidatableObject
{
    [Range(1,9999, ErrorMessage = "Quantity must be between 1 and 9999")]
    public int Quantity { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CreatedAt.Date <= DateTime.Now.Date)
        {
            yield return new ValidationResult("Create Date must be greater than the current date.",
                new[] { nameof(this.CreatedAt) });
        }
    }
}
