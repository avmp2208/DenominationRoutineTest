using System.ComponentModel.DataAnnotations;

namespace RestServerTest.Common;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class AgeValidatorAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null && (int)value > 18)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("Age must be above 18.");
    }
}