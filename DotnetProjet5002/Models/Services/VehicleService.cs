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
                Year = v.Year,
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

        public async Task<VehicleViewModel> GetVehicleByCodeVinAsync(string codeVin)
        {
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(m => m.CodeVin == codeVin);
            if (vehicle == null)
            {
                return null;
            }

         
            return new VehicleViewModel
            {
                Year = vehicle.Year,
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

        public async Task CreateVehicleAsync(VehicleViewModel vehicleViewModel, IFormFile imageFile)
        {


            if (imageFile != null)
            {
                vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(imageFile);
            }

            // Calculer le coût total des réparations
            var totalRepairCost = await CalculateTotalRepairCostAsync(vehicleViewModel.CodeVin);

            // Calculer le prix de vente
            var sellPrice = vehicleViewModel.PurchasePrice + totalRepairCost + 500;
            var vehicle = new Vehicle
            {
                CodeVin = vehicleViewModel.CodeVin,
                Year = vehicleViewModel.Year,
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
                SellPrice = sellPrice
            };
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(VehicleViewModel vehicleViewModel, IFormFile imageFile)
        {
            var vehicle = await _context.Vehicle.FindAsync(vehicleViewModel.CodeVin);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }
            if (imageFile != null)
            {
                vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(imageFile);
            }

            // Calculer le coût total des réparations
            var totalRepairCost = await CalculateTotalRepairCostAsync(vehicleViewModel.CodeVin);

            // Calculer le prix de vente
            var sellPrice = vehicleViewModel.PurchasePrice + totalRepairCost + 500;

            vehicle.Year = vehicleViewModel.Year;
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
            vehicle.SellPrice = vehicleViewModel.SellPrice;

            _context.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(string codeVin)
        {
            var vehicle = await _context.Vehicle.FindAsync(codeVin);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<float> CalculateTotalRepairCostAsync(string codeVin)
        {
            var repairs = await _repairService.GetRepairsByVehicleAsync(codeVin);
            return repairs.Sum(repair => repair.RepairCost);
        }


        public async Task<bool> VehicleExistsAsync(string codeVin)
        {
            return await _context.Vehicle.AnyAsync(e => e.CodeVin == codeVin);
        }
    }
}

