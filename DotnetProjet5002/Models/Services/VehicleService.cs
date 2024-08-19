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

        public VehicleService(ApplicationDbContext context, IRepairService repairService)
        {
            _context = context;
            _repairService = repairService;
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
                CodeVin = v.CodeVin
            }).ToList();
        }

        public async Task<VehicleViewModel> GetVehicleByCodeVinAsync(string CodeVin)
        {
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(m => m.CodeVin == CodeVin);
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
                CodeVin = vehicle.CodeVin
            };
        }

        public async Task CreateVehicleAsync(VehicleViewModel vehicleViewModel)
        {
            var vehicle = new Vehicle
            {
                CodeVin = vehicleViewModel.CodeVin,
                Year = vehicleViewModel.Year,
                Brand = vehicleViewModel.Brand,
                Model = vehicleViewModel.Model,
                Finish = vehicleViewModel.Finish,
                Availability = vehicleViewModel.Availability,
                AvailabilityDate = vehicleViewModel.AvailabilityDate,
                Description = vehicleViewModel.Description, // Added the missing property
                ImageUrl = vehicleViewModel.ImageUrl
            };
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(VehicleViewModel vehicleViewModel)
        {
            var vehicle = await _context.Vehicle.FindAsync(vehicleViewModel.CodeVin);
            if (vehicle == null)
            {
                throw new Exception("Vehicle not found");
            }

            vehicle.Year = vehicleViewModel.Year;
            vehicle.Brand = vehicleViewModel.Brand;
            vehicle.Model = vehicleViewModel.Model;
            vehicle.Finish = vehicleViewModel.Finish;
            vehicle.Availability = vehicleViewModel.Availability;
            vehicle.AvailabilityDate = vehicleViewModel.AvailabilityDate;
            vehicle.Description = vehicleViewModel.Description; // Added the missing property
        }

        public async Task DeleteVehicleAsync(string CodeVin)
        {
            var vehicle = await _context.Vehicle.FindAsync(CodeVin);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<VehicleViewModel>> CalculateSellPriceAsync()
        {
            var vehicles = await _context.Vehicle.ToListAsync();
            var vehicleViewModels = new List<VehicleViewModel>();

            foreach (var vehicle in vehicles)
            {
                var repairs = await _repairService.GetRepairsByVehicleAsync(vehicle.CodeVin);

                float totalRepairCost = repairs.Sum(r => r.RepairCost);// filtrer les réparations par le code vin du véhicule et sommer les coûts de réparation
                float sellPrice = vehicle.PurchasePrice + totalRepairCost + 500;

                vehicleViewModels.Add(new VehicleViewModel
                {
                    CodeVin = vehicle.CodeVin,
                    Year = vehicle.Year,
                    PurchaseDate = vehicle.PurchaseDate,
                    PurchasePrice = vehicle.PurchasePrice,
                    Brand = vehicle.Brand,
                    Model = vehicle.Model,
                    Finish = vehicle.Finish,
                    Description = vehicle.Description,
                   // SellPrice = sellPrice,
                    Availability = vehicle.Availability,
                    ImageUrl = vehicle.ImageUrl,
                    AvailabilityDate = vehicle.AvailabilityDate,
                    Selled = vehicle.Selled,
                   // Repairs = repairs
                });

                // Console.WriteLine($"Vehicle CodeVin: {vehicle.CodeVin}, SellPrice: {sellPrice}");
            }

            return vehicleViewModels;
        }
    }
}

