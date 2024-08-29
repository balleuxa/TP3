using System.ComponentModel.DataAnnotations;

namespace BanqueTardiApp.Validations
{
    public class AgeMinimumAttribute : ValidationAttribute
    {
        public AgeMinimumAttribute(int ageMinimum) 
        {
            _ageMinimum = ageMinimum;
        }
        private readonly int _ageMinimum;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateNaissance)
            {
                var age = DateTime.Today.Year - dateNaissance.Year;
                if (dateNaissance > DateTime.Today.AddYears(-age)) age--;

                if (age < _ageMinimum)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

}
