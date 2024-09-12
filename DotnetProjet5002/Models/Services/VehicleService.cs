using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.Entities;
using DotnetProjet5.ViewModels;

using DotnetProjet5.Services;

namespace DotnetProjet5.Models.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepairService _repairService;
        private readonly IFileUploadHelper _fileUploadHelper;

        public VehicleService(ApplicationDbContext context, IRepairService repairService, IFileUploadHelper fileUploadHelper)
        {
            _context = context;
            _repairService = repairService;
            _fileUploadHelper = fileUploadHelper;
        }

        public async Task<List<VehicleViewModel>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicle.ToListAsync();
            return vehicles.Select(v => new VehicleViewModel
            {
                VehicleId = v.VehicleId,
                Year = v.Year.Year,
                Brand = v.Brand,
                Model = v.Model,
                Finish = v.Finish,
                Availability = v.Availability,
                AvailabilityDate = v.AvailabilityDate ?? DateTime.MinValue,
                CodeVin = v.CodeVin,
                Description = v.Description ?? string.Empty,
                ImageUrl = v.ImageUrl,
                PurchaseDate = v.PurchaseDate,
                PurchasePrice = v.PurchasePrice,
                Selled = v.Selled,
                SellPrice = v.SellPrice
            }).ToList();
        }

        public async Task<VehicleViewModel> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
            if (vehicle == null)
            {
                return null;
            }

            return new VehicleViewModel
            {
                VehicleId = vehicle.VehicleId, // Assurez-vous que cette ligne est présente
                Year = vehicle.Year.Year,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Finish = vehicle.Finish,
                Availability = vehicle.Availability,
                AvailabilityDate = vehicle.AvailabilityDate ?? DateTime.MinValue,
                CodeVin = vehicle.CodeVin,
                Description = vehicle.Description ?? string.Empty,
                ImageUrl = vehicle.ImageUrl,
                PurchaseDate = vehicle.PurchaseDate,
                PurchasePrice = vehicle.PurchasePrice,
                Selled = vehicle.Selled,
                SellPrice = vehicle.SellPrice
            };
        }

        public async Task<List<VehicleViewModel>> GetAllVehiclesAvailableAsync()
        {
            var vehicles = await _context.Vehicle.Where(v => v.Availability == true && v.Selled == false).ToListAsync();
            return vehicles.Select(v => new VehicleViewModel
            {
                VehicleId = v.VehicleId,
                Year = v.Year.Year,
                Brand = v.Brand,
                Model = v.Model,
                Finish = v.Finish,
                Availability = v.Availability,
                AvailabilityDate = v.AvailabilityDate ?? DateTime.MinValue,
                CodeVin = v.CodeVin,
                Description = v.Description ?? string.Empty,
                ImageUrl = v.ImageUrl,
                PurchaseDate = v.PurchaseDate,
                PurchasePrice = v.PurchasePrice,
                Selled = v.Selled,
                SellPrice = v.SellPrice
            }).ToList();
        }

        public async Task CreateVehicleAsync(VehicleViewModel vehicleViewModel)
        {
            if (vehicleViewModel.ImageFile != null)
            {
                vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(vehicleViewModel.ImageFile);
            }

            var totalRepairCost = await CalculateTotalRepairCostAsync(vehicleViewModel.VehicleId);
            var sellPrice = vehicleViewModel.PurchasePrice + totalRepairCost + 500;
            var yearAsDateTime = new DateTime(vehicleViewModel.Year, 1, 1);

            var vehicle = new Vehicle
            {
                CodeVin = vehicleViewModel.CodeVin,
                Brand = vehicleViewModel.Brand,
                Model = vehicleViewModel.Model,
                Finish = vehicleViewModel.Finish,
                Availability = vehicleViewModel.Availability,
                AvailabilityDate = vehicleViewModel.AvailabilityDate,
                Description = vehicleViewModel.Description,
                ImageUrl = vehicleViewModel.ImageUrl,
                PurchaseDate = vehicleViewModel.PurchaseDate,
                PurchasePrice = vehicleViewModel.PurchasePrice,
                Selled = vehicleViewModel.Selled,
                SellPrice = sellPrice,
                Year = yearAsDateTime
            };

            _context.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(VehicleViewModel vehicleViewModel)
        {
            var vehicle = await _context.Vehicle.FindAsync(vehicleViewModel.VehicleId);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }
            if (vehicleViewModel.ImageFile != null)
            {
                vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(vehicleViewModel.ImageFile);
            }

            var totalRepairCost = await CalculateTotalRepairCostAsync(vehicleViewModel.VehicleId);
            var sellPrice = vehicleViewModel.PurchasePrice + totalRepairCost + 500;
            var yearAsDateTime = new DateTime(vehicleViewModel.Year, 1, 1);

            vehicle.VehicleId = vehicleViewModel.VehicleId;
            vehicle.CodeVin = vehicleViewModel.CodeVin;
            vehicle.Brand = vehicleViewModel.Brand;
            vehicle.Model = vehicleViewModel.Model;
            vehicle.Finish = vehicleViewModel.Finish;
            vehicle.Availability = vehicleViewModel.Availability;
            vehicle.AvailabilityDate = vehicleViewModel.AvailabilityDate;
            vehicle.Description = vehicleViewModel.Description;
            vehicle.ImageUrl = vehicleViewModel.ImageUrl;
            vehicle.PurchaseDate = vehicleViewModel.PurchaseDate;
            vehicle.PurchasePrice = vehicleViewModel.PurchasePrice;
            vehicle.Selled = vehicleViewModel.Selled;
            vehicle.SellPrice = sellPrice;
            vehicle.Year = yearAsDateTime;

            _context.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            await _repairService.DeleteRepairsByVehicleAsync(vehicleId);
            var vehicle = await _context.Vehicle.FindAsync(vehicleId);
            if (vehicle != null)
            {
                if (!string.IsNullOrEmpty(vehicle.ImageUrl))
                {
                    var imagePath = Path.Combine("wwwroot/images", Path.GetFileName(vehicle.ImageUrl));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<float> CalculateTotalRepairCostAsync(int vehicleId)
        {
            var repairs = await _repairService.GetRepairsByVehicleAsync(vehicleId);
            return repairs.Sum(repair => repair.RepairCost);
        }

        public async Task<bool> VehicleExistsAsync(int vehicleId)
        {
            return await _context.Vehicle.AnyAsync(e => e.VehicleId == vehicleId);
        }
    }
}

