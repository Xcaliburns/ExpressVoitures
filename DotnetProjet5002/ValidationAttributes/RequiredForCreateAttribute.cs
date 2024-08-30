using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DotnetProjet5.ValidationAttributes
{
    public class RequiredForCreateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var httpContext = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            var request = httpContext.HttpContext.Request;

            // Check if the request is for creating a new vehicle
            if (request.Method == HttpMethods.Post && request.Path.Value.Contains("Create"))
            {
                if (value == null || !(value is IFormFile file) || file.Length == 0)
                {
                    return new ValidationResult(ErrorMessage ?? "Veuillez ajouter une image.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
