using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BanqueTardiApp.Validations;

public class ZipCodeCAAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string postalCode)
        {
            Regex codePostalCanadien = new Regex("^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$");

            if (!codePostalCanadien.IsMatch(postalCode))
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
