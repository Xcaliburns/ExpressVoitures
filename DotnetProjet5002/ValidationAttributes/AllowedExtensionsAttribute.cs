using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DotnetProjet5.ValidationAttributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (Array.IndexOf(_extensions, extension) < 0)
                {
                    return new ValidationResult($"Les fichiers autorisés sont: {string.Join(", ", _extensions)}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
