using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DotnetProjet5.Models.Services
{
    public class FileUploadHelper : IFileUploadHelper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadHelper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }


            // Générer un nom de fichier unique
            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            // Créer le répertoire de stockage s'il n'existe pas
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Télécharger le fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Retourner l'URL relative du fichier
            return $"/images/{uniqueFileName}";
        }
    }
}
