using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotnetProjet5.Models.Services
{
    public interface IFileUploadHelper
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteFileIfExistsAsync(string filePath);
    }
}
