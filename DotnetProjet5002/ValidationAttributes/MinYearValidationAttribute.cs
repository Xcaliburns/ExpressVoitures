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
            if (value is int year)
            {
                if (year < _minYear)
                {
                    return new ValidationResult($"L'année doit être supérieure ou égale à {_minYear}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
