using DotnetProjet5.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DotnetProjet5.ValidationAttributes
{
    public class UniqueVINAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Veuillez entrer un code VIN valide.");
            }

            var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var vehicleIdProperty = validationContext.ObjectType.GetProperty("VehicleId");

           

            var currentVehicleId = (int)vehicleIdProperty.GetValue(validationContext.ObjectInstance, null);
            var existingVIN = context.Vehicle.Any(v => v.CodeVin == value.ToString() && v.VehicleId != currentVehicleId);

            if (existingVIN)
            {
                return new ValidationResult("Ce code VIN existe déjà.");
            }

            return ValidationResult.Success;
        }
    }
   
}