using DotnetProjet5.Data;
using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using DotnetProjet5.Models.Services;
using DotnetProjet5.ViewModels;
using Elfie.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotnetProjet5.UnitTests
{
    public class VehicleServiceTests
    {


        private readonly Mock<ApplicationDbContext> _mockContext;
        private readonly Mock<IRepairService> _mockRepairService;
        private readonly Mock<IFileUploadHelper> _mockFileUploadHelper;
        private readonly Mock<ILogger<VehicleService>> _mockLogger;
        private readonly VehicleService _vehicleService;

        public VehicleServiceTests()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _mockRepairService = new Mock<IRepairService>();
            _mockFileUploadHelper = new Mock<IFileUploadHelper>();
            _mockLogger = new Mock<ILogger<VehicleService>>();
            _vehicleService = new VehicleService(
             _mockContext.Object,
             _mockRepairService.Object,
             _mockFileUploadHelper.Object,
             _mockLogger.Object
            );
        }



        [Fact]
        public async Task GetAllVehiclesAsync_ReturnsVehicleList()
        {
            // Arrange
            var vehicles = new List<Vehicle>
    {
        new Vehicle
        {
            VehicleId = 1,
            CodeVin = "1HGCM82633A123456",
            Year = new DateTime(2020, 1, 1),
            PurchaseDate = new DateTime(2020, 1, 1),
            PurchasePrice = 10000.0f,
            Brand = "Brand1",
            Model = "Model1",
            Finish = "Finish1",
            Description = "Description1",
            Availability = true,
            ImageUrl = "imageUrl",
            AvailabilityDate = new DateTime(2020, 1, 1),
            SellPrice = 15000.0f,
            Selled = false
        },
        new Vehicle
        {
            VehicleId = 2,
            CodeVin = "1HGCM82633A654321",
            Year = new DateTime(2021, 1, 1),
            PurchaseDate = new DateTime(2021, 1, 1),
            PurchasePrice = 12000.0f,
            Brand = "Brand2",
            Model = "Model2",
            Finish = "Finish2",
            Description = "Description2",
            Availability = true,
            ImageUrl = "imageUrl2",
            AvailabilityDate = new DateTime(2021, 1, 1),
            SellPrice = 16000.0f,
            Selled = false
        }
    }.AsQueryable();

            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(vehicles.Provider);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(vehicles.Expression);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(vehicles.ElementType);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(vehicles.GetEnumerator());

            // Mock the IAsyncEnumerable interface
            mockSet.As<IAsyncEnumerable<Vehicle>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Vehicle>(vehicles.GetEnumerator()));

            _mockContext.Setup(c => c.Set<Vehicle>()).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.GetAllVehiclesAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Brand1", result[0].Brand);
            Assert.Equal("Brand2", result[1].Brand);
        }


        // Helper class to mock IAsyncEnumerator
        public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            public T Current => _inner.Current;
        }





        [Fact]
        public async Task GetVehicleByIdAsync_ReturnsVehicle()
        {
            // Arrange
            var vehicle = new Vehicle { VehicleId = 1, Brand = "Brand1", Model = "Model1", Year = new DateTime(2020, 1, 1) };

            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);

            _mockContext.Setup(c => c.Set<Vehicle>()).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.GetVehicleByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Brand1", result.Brand);
        }

        [Fact]
        public async Task GetAllVehiclesAvailableAsync_ReturnsAvailableVehicles()
        {
            // Arrange
            var vehicles = new List<Vehicle>
            {
                new Vehicle { VehicleId = 1, Brand = "Brand1", Model = "Model1", Year = new DateTime(2020, 1, 1), Availability = true, Selled = false },
                new Vehicle { VehicleId = 2, Brand = "Brand2", Model = "Model2", Year = new DateTime(2021, 1, 1), Availability = false, Selled = true }
            };

            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(vehicles.AsQueryable().Provider);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(vehicles.AsQueryable().Expression);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(vehicles.AsQueryable().ElementType);
            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(vehicles.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Set<Vehicle>()).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.GetAllVehiclesAvailableAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Brand1", result[0].Brand);
        }

        [Fact]
        public async Task CreateVehicleAsync_CreatesVehicle()
        {
            // Arrange
            var vehicleViewModel = new VehicleViewModel
            {
                VehicleId = 1,
                Brand = "Brand1",
                Model = "Model1",
                Year = 2020,
                PurchasePrice = 10000,
                ImageFile = null
            };

            _mockFileUploadHelper.Setup(f => f.UploadFileAsync(It.IsAny<IFormFile>())).ReturnsAsync("imageUrl");

            // Act
            await _vehicleService.CreateVehicleAsync(vehicleViewModel);

            // Assert
            _mockContext.Verify(c => c.Add(It.IsAny<Vehicle>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateVehicleAsync_UpdatesVehicle()
        {
            // Arrange
            var vehicle = new Vehicle { VehicleId = 1, Brand = "Brand1", Model = "Model1", Year = new DateTime(2020, 1, 1) };
            var vehicleViewModel = new VehicleViewModel
            {
                VehicleId = 1,
                Brand = "UpdatedBrand",
                Model = "UpdatedModel",
                Year = 2020,
                PurchasePrice = 10000,
                ImageFile = null
            };

            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);

            _mockContext.Setup(c => c.Set<Vehicle>()).Returns(mockSet.Object);

            // Act
            await _vehicleService.UpdateVehicleAsync(vehicleViewModel);

            // Assert
            _mockContext.Verify(c => c.Update(It.IsAny<Vehicle>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteVehicleAsync_DeletesVehicle()
        {
            // Arrange
            var vehicle = new Vehicle
            {
                VehicleId = 1,
                CodeVin = "1HGCM82633A123456", // Example VIN
                Year = new DateTime(2020, 1, 1),
                PurchaseDate = new DateTime(2020, 1, 1),
                PurchasePrice = 10000.0f,
                Brand = "Brand1",
                Model = "Model1",
                Finish = "Finish1",
                Description = "Description1",
                Availability = true,
                ImageUrl = "imageUrl",
                AvailabilityDate = new DateTime(2020, 1, 1),
                SellPrice = 15000.0f,
                Selled = false
            };

            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(vehicle);
            mockSet.Setup(m => m.Remove(It.IsAny<Vehicle>()));

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Vehicle).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var mockRepairService = new Mock<IRepairService>();
            mockRepairService.Setup(r => r.DeleteRepairsByVehicleAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            var mockFileUploadHelper = new Mock<IFileUploadHelper>();
            var mockLogger = new Mock<ILogger<VehicleService>>();

            var vehicleService = new VehicleService(mockContext.Object, mockRepairService.Object, mockFileUploadHelper.Object, mockLogger.Object);

            // Act
            await vehicleService.DeleteVehicleAsync(1);

            // Assert
            Assert.NotNull(mockContext.Object);
            mockSet.Verify(m => m.Remove(It.IsAny<Vehicle>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockRepairService.Verify(r => r.DeleteRepairsByVehicleAsync(It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async Task CalculateTotalRepairCostAsync_ReturnsTotalCost()
        {
            // Arrange
            var repairs = new List<Repair>
                {
                    new Repair { RepairId = 1, VehicleId = 1, RepairCost = 100 },
                    new Repair { RepairId = 2, VehicleId = 1, RepairCost = 200 }
                };

            _mockRepairService.Setup(r => r.GetRepairsByVehicleAsync(1)).ReturnsAsync(repairs);

            // Act
            var result = await _vehicleService.CalculateTotalRepairCostAsync(1);

            // Assert
            Assert.Equal(300, result);
        }

        [Fact]
        public async Task VehicleExistsAsync_ReturnsTrueIfExists()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Vehicle>>();
            mockSet.Setup(m => m.AnyAsync(It.IsAny<Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _mockContext.Setup(c => c.Set<Vehicle>()).Returns(mockSet.Object);

            // Act
            var result = await _vehicleService.VehicleExistsAsync(1);

            // Assert
            Assert.True(result);
        }
    }
}
