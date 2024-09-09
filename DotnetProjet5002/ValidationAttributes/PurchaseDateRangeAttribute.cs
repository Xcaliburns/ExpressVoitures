using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ValidationAttributes
{
    public class PurchaseDateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = instance.GetType();
            var yearProperty = type.GetProperty("Year");

            if (yearProperty != null && yearProperty.GetValue(instance) is DateTime yearDateTime)
            {
                if (value is DateTime purchaseDate)
                {
                    if (purchaseDate < DateTime.Now.Date || purchaseDate.Year > yearDateTime.Year)
                    {
                        return new ValidationResult($"La date d'achat doit être comprise entre aujourd'hui et l'année du véhicule ({yearDateTime.Year}).");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}

