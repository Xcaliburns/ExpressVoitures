using System;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ValidationAttributes

{
    public class MinYearValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int year)
            {
                return year <= DateTime.Now.Year;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"L'année doit être inférieure ou égale à l'année en cours ({DateTime.Now.Year}).";
        }
    }
}
