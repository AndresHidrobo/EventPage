using System.ComponentModel.DataAnnotations;

namespace Eventual.Models;

public class FutureDateAttribute : ValidationAttribute
{
    public FutureDateAttribute() 
        : base("La fecha del evento debe ser hoy o en el futuro.") { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime date)
        {
            if (date.Date < DateTime.Today)
                return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}
