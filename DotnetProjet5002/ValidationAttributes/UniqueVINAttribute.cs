using DotnetProjet5.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotnetProjet5.ValidationAttributes
{
    public class UniqueVINAttribute :  ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {//TODO gerer la nullité
            if (value == null)
            {
                return new ValidationResult("veuiilez entrer un codeVin valide.");
            }
               
          
            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var existingVIN = context.Vehicle.Any(v => v.CodeVin == value.ToString());
            
            if (existingVIN)
            {
                return new ValidationResult("Ce code VIN existe déjà.");
            }  
            return ValidationResult.Success;
        }
    }
   
}