//using System;
//using System.Threading.Tasks;
//using DotnetProjet5.Controllers;
//using DotnetProjet5.Data;
//using DotnetProjet5.Models.Entities;
//using DotnetProjet5.Models.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using Xunit;

//namespace DotnetProjet5.Controllers.Tests
//{
//    public class VehiclesControllerTests
//    {

//        private readonly Mock<ApplicationDbContext> _mockContext;
//        private readonly VehiclesController _controller;

//        public VehiclesControllerTests()
//        {
//            _mockContext = new Mock<ApplicationDbContext>();
//            _controller = new VehiclesController(_mockContext.Object);
//        }
//        [Fact()]
//        public void VehiclesControllerTest()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }

//        [Fact]
//        public async Task Index_ReturnsViewResult_WithListOfVehicleViewModels()
//        {
//            // Arrange
//            var vehicles = new List<Vehicle>
//            {
//                new Vehicle { CodeVin = "123ABC", Year = new DateTime(2022, 1, 1), Brand = "TestBrand1", Model = "TestModel1", Finish = "TestFinish1", Availability = true, AvailabilityDate = DateTime.Now },
//                new Vehicle { CodeVin = "456DEF", Year = new DateTime(2021, 1, 1), Brand = "TestBrand2", Model = "TestModel2", Finish = "TestFinish2", Availability = false, AvailabilityDate = DateTime.Now.AddDays(1) }
//            };

//            var mockSet = new Mock<DbSet<Vehicle>>();
//            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(vehicles.AsQueryable().Provider);
//            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(vehicles.AsQueryable().Expression);
//            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(vehicles.AsQueryable().ElementType);
//            mockSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(vehicles.AsQueryable().GetEnumerator());

//            _mockContext.Setup(c => c.Vehicle).Returns(mockSet.Object);

//            // Act
//            var result = await _controller.Index();

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<List<VehicleViewModel>>(viewResult.ViewData.Model);
//            Assert.Equal(2, model.Count);
//            Assert.Equal("123ABC", model[0].CodeVin);
//            Assert.Equal("456DEF", model[1].CodeVin);
//        }

//        [Fact()]
//        public void DetailsTest()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }

//        [Fact]
//        public async Task Create_ValidModelState_ReturnsRedirectToActionResult()
//        {
//            // Arrange
//            var vehicleViewModel = new VehicleViewModel
//            {
//                CodeVin = "123ABC",
//                Year = 2022,
//                Brand = "TestBrand",
//                Model = "TestModel",
//                Finish = "TestFinish",
//                Availability = true,
//                AvailabilityDate = DateTime.Now
//            };

//            // Act
//            var result = await _controller.Create(vehicleViewModel);

//            // Assert
//            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("Index", redirectToActionResult.ActionName);
//            _mockContext.Verify(m => m.Add(It.IsAny<Vehicle>()), Times.Once);
//            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
//        }

//        [Fact]
//        public async Task Create_InvalidModelState_ReturnsViewResult()
//        {
//            // Arrange
//            _controller.ModelState.AddModelError("CodeVin", "Required");
//            var vehicleViewModel = new VehicleViewModel();

//            // Act
//            var result = await _controller.Create(vehicleViewModel);

//            // Assert
//            var viewResult = Assert.IsType<ViewResult>(result);
//            Assert.Equal(vehicleViewModel, viewResult.Model);
//            _mockContext.Verify(m => m.Add(It.IsAny<Vehicle>()), Times.Never);
//            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
//        }

        

//        [Fact()]
//        public void EditTest()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }

//        [Fact()]
//        public void EditTest1()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }

//        [Fact()]
//        public void DeleteTest()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }

//        [Fact()]
//        public void DeleteConfirmedTest()
//        {
//            Xunit.Assert.Fail("This test needs an implementation");
//        }
//    }
//}