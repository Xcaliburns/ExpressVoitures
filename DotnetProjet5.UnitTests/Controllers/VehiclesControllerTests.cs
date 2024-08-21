using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DotnetProjet5.Controllers;
using DotnetProjet5.Models.Services;
using DotnetProjet5.ViewModels;
using DotnetProjet5.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetProjet5.UnitTests.ControllersTests
{
    public class VehiclesControllerTests
    {
        [Fact]
        public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mockVehicleService = new Mock<IVehicleService>();
            var mockRepairService = new Mock<IRepairService>();
            var mockFileUploadHelper = new Mock<IFileUploadHelper>();

            var controller = new VehiclesController(context, mockVehicleService.Object, mockRepairService.Object, mockFileUploadHelper.Object);

            var vehicleViewModel = new VehicleViewModel
            {
                CodeVin = "1234567890",
                Brand = "TestBrand",
                Model = "TestModel",
                Year = new DateTime(2021, 1, 1),
                PurchaseDate = new DateTime(2021, 1, 1),
                PurchasePrice = 10000,
                Description = "Test Description",
                ImageFile = new Mock<IFormFile>().Object
            };

            var mockFormFile = new Mock<IFormFile>();

            // Act
            var result = await controller.Create(vehicleViewModel, mockFormFile.Object);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new ApplicationDbContext(options);
            var mockVehicleService = new Mock<IVehicleService>();
            var mockRepairService = new Mock<IRepairService>();
            var mockFileUploadHelper = new Mock<IFileUploadHelper>();

            var controller = new VehiclesController(context, mockVehicleService.Object, mockRepairService.Object, mockFileUploadHelper.Object);
            controller.ModelState.AddModelError("Brand", "Required");

            var vehicleViewModel = new VehicleViewModel
            {
                CodeVin = "1234567890",
                Model = "TestModel",
                Year = new DateTime(2021, 1, 1),
                PurchaseDate = new DateTime(2021, 1, 1),
                PurchasePrice = 10000,
                Description = "Test Description",
                ImageFile = new Mock<IFormFile>().Object
            };

            var mockFormFile = new Mock<IFormFile>();

            // Act
            var result = await controller.Create(vehicleViewModel, mockFormFile.Object);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(vehicleViewModel, viewResult.Model);
        }
    }
}