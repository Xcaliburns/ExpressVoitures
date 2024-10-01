using DotnetProjet5.Data;
using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using DotnetProjet5.Models.Services;
using DotnetProjet5.Models.ViewModels;
using DotnetProjet5.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DotnetProjet5.UnitTests
{
    public class RepairServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<ILogger<RepairService>> _mockLogger;
        private readonly RepairService _repairService;

        public RepairServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _mockLogger = new Mock<ILogger<RepairService>>();
            _repairService = new RepairService(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetRepairsByVehicleAsync_ReturnsRepairs()
        {
            // Arrange
            var vehicleId = 1;
            var repairs = new List<Repair>
            {
                new Repair { RepairId = 1, VehicleId = vehicleId, Description = "Repair 1", RepairCost = 100 },
                new Repair { RepairId = 2, VehicleId = vehicleId, Description = "Repair 2", RepairCost = 200 }
            };

            _context.Repairs.AddRange(repairs);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repairService.GetRepairsByVehicleAsync(vehicleId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Repair 1", result[0].Description);
            Assert.Equal("Repair 2", result[1].Description);
        }

        [Fact]
        public async Task AddRepairAsync_AddsRepair()
        {
            // Arrange
            var repair = new Repair { RepairId = 1, VehicleId = 1, Description = "Repair 1", RepairCost = 100 };

            // Act
            await _repairService.AddRepairAsync(repair);

            // Assert
            var addedRepair = await _context.Repairs.FindAsync(repair.RepairId);
            Assert.NotNull(addedRepair);
            Assert.Equal("Repair 1", addedRepair.Description);
        }

        [Fact]
        public async Task DeleteRepairByIdAsync_DeletesRepair()
        {
            // Arrange
            var repair = new Repair { RepairId = 1, VehicleId = 1, Description = "Repair 1", RepairCost = 100 };
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();

            // Act
            await _repairService.DeleteRepairByIdAsync(repair.RepairId);

            // Assert
            var deletedRepair = await _context.Repairs.FindAsync(repair.RepairId);
            Assert.Null(deletedRepair);
        }

        [Fact]
        public async Task RepairExistsAsync_ReturnsTrueIfExists()
        {
            // Arrange
            var repairId = 1;
            var repair = new Repair { RepairId = repairId, VehicleId = 1, Description = "Repair 1", RepairCost = 100 };
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repairService.RepairExistsAsync(repairId);

            // Assert
            Assert.True(result);
        }
    }
}
