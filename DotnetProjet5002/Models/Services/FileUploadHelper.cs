using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotnetProjet5.Models.Services
{
    public class FileUploadHelper : IFileUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<FileUploadHelper> _logger;

        public FileUploadHelper(IWebHostEnvironment webHostEnvironment, ILogger<FileUploadHelper> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return string.Empty;
                }

                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Create the directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Upload the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the relative URL of the file
                return $"/images/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"Une erreur s'est produite lors du téléchargement du fichier : {ex.Message}");
                return string.Empty;
            }
        }

        public async Task DeleteFileIfExistsAsync(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    await Task.Run(() => System.IO.File.Delete(filePath));
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"Une erreur s'est produite lors de la suppression du fichier : {ex.Message}");
            }
        }
    }
}
