using System.ComponentModel.DataAnnotations;

namespace APIProductCatalog.Validations;

public class StartWithUpperCaseAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var firstChar = value.ToString().FirstOrDefault().ToString();
        if (firstChar != firstChar.ToUpper())
            return new ValidationResult("The first letter must be capitalized.");


        return ValidationResult.Success;
    }
}
