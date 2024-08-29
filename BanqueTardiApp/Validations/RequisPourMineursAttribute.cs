using BanqueTardiApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BanqueTardiApp.Validations
{
    public class RequisPourMineurAttribute : ValidationAttribute
    {
        private const int AGE_MINIMUM = 15;
        private const int AGE_MAXIMUM = 18;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var client = (Client)validationContext.ObjectInstance;
            var aujourdhui = DateTime.Today;
            var age = aujourdhui.Year - client.DateNaissance.Year;
            if (client.DateNaissance.Date > aujourdhui.AddYears(-age)) age--;

            if (age >= AGE_MINIMUM && age < AGE_MAXIMUM)
            {
                if (string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
