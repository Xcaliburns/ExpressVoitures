using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ValidationAttributes
{
    public class MaxDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Date > DateTime.Now.Date)
                {
                    return new ValidationResult("La date  peut  être aujourd'hui au plus tard.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
