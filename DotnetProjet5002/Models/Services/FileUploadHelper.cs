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

                
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
                return $"/images/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                
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
                
                _logger.LogError(ex, $"Une erreur s'est produite lors de la suppression du fichier : {ex.Message}");
            }
        }
    }
}
