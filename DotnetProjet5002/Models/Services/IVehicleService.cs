
using DotnetProjet5.Models.Entities;
using DotnetProjet5.ViewModels;

namespace DotnetProjet5.Models.Services
{
    public interface IVehicleService
    {
        Task<List<VehicleViewModel>> GetAllVehiclesAsync();
        Task<VehicleViewModel> GetVehicleByCodeVinAsync(string CodeVin);
        Task CreateVehicleAsync(VehicleViewModel vehicleViewModel);
        Task UpdateVehicleAsync(VehicleViewModel vehicleViewModel);
        Task DeleteVehicleAsync(string CodeVin);
         Task<float> CalculateTotalRepairCostAsync(string codeVin);
        
    }

}
