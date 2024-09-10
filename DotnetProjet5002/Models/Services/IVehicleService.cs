
using DotnetProjet5.Models.Entities;
using DotnetProjet5.ViewModels;

namespace DotnetProjet5.Models.Services
{
    public interface IVehicleService
    {
        Task<List<VehicleViewModel>> GetAllVehiclesAsync();
        Task<VehicleViewModel> GetVehicleByIdAsync(int vehicleId);
        Task<List<VehicleViewModel>> GetAllVehiclesAvailableAsync();
        Task CreateVehicleAsync(VehicleViewModel VehicleViewModel);
        Task UpdateVehicleAsync(VehicleViewModel VehicleViewModel);
        Task DeleteVehicleAsync(int vehicleId);
        Task<float> CalculateTotalRepairCostAsync(int vehicleId);
        Task<bool> VehicleExistsAsync(int vehicleId);

    }

}
