using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ValidationAttributes

{
    public class MinYearValidationAttribute : ValidationAttribute
    {
        private readonly int _minYear;

        public MinYearValidationAttribute(int minYear)
        {
            _minYear = minYear;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                int year = dateTime.Year;
                int currentYear = DateTime.Now.Year;

                if (year < _minYear )
                {
                    return new ValidationResult($"L'année doit être supérieure à {_minYear}");
                }
                if (year > currentYear)
                {
                    return new ValidationResult("L'année doit être inférieure ou égale à l'année en cours.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
