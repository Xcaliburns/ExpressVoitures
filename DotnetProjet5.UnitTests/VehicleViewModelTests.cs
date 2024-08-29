using System;
using System.Collections.Generic;
using Xunit;
using DotnetProjet5.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.Tests.ViewModels
{
    public class VehicleViewModelTests
    {
        [Fact]
        public void PurchasePrice_ShouldHaveRangeValidation()
        {
            // Arrange
            var vehicleViewModel = new VehicleViewModel();

            // Act
            var validationContext = new ValidationContext(vehicleViewModel)
            {
                MemberName = nameof(VehicleViewModel.PurchasePrice)
            };
            var validationResults = new List<ValidationResult>();

            // Test with 2.25
            vehicleViewModel.PurchasePrice = 2.25f;
            var isValid1 = Validator.TryValidateProperty(vehicleViewModel.PurchasePrice, validationContext, validationResults);

           
           
            

            // Assert
            Assert.False(isValid1);
            
            Assert.Equal("merci d'entrer un prix valide", validationResults[0].ErrorMessage);
        }
    }
}
