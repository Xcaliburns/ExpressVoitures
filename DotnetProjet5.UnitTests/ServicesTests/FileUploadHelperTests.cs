using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using DotnetProjet5.Models.Services;

namespace DotnetProjet5.UnitTests.ServicesTests
{
    public class FileUploadHelperTests
    {
        [Fact]
        public async Task UploadFileAsync_ShouldReturnFilePath_WhenFileIsValid()
        {
            // Arrange
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("wwwroot");

            var mockFormFile = new Mock<IFormFile>();
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write("Test file content");
            writer.Flush();
            ms.Position = 0;

            mockFormFile.Setup(f => f.FileName).Returns(fileName);
            mockFormFile.Setup(f => f.Length).Returns(ms.Length);
            mockFormFile.Setup(f => f.OpenReadStream()).Returns(ms);
            mockFormFile.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>())).Returns((Stream target, CancellationToken token) =>
            {
                ms.CopyTo(target);
                return Task.CompletedTask;
            });

            var fileUploadHelper = new FileUploadHelper(mockWebHostEnvironment.Object);

            // Act
            var result = await fileUploadHelper.UploadFileAsync(mockFormFile.Object);

            // Assert
            Assert.False(string.IsNullOrEmpty(result));
            Assert.StartsWith("/images/", result);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldReturnEmptyString_WhenFileIsNull()
        {
            // Arrange
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var fileUploadHelper = new FileUploadHelper(mockWebHostEnvironment.Object);

            // Act
            var result = await fileUploadHelper.UploadFileAsync(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldReturnEmptyString_WhenFileIsEmpty()
        {
            // Arrange
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var mockFormFile = new Mock<IFormFile>();
            mockFormFile.Setup(f => f.Length).Returns(0);

            var fileUploadHelper = new FileUploadHelper(mockWebHostEnvironment.Object);

            // Act
            var result = await fileUploadHelper.UploadFileAsync(mockFormFile.Object);

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
