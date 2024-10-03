using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ValidationAttributes
{
    public class MaxYearValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int year)
            {
                int currentYear = DateTime.Now.Year;
                if (year > currentYear)
                {
                    return new ValidationResult($"L'année doit être inférieure ou égale à {currentYear}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
