using DotnetProjet5.Models.Entities;
using DotnetProjet5.Models.ViewModels;

namespace DotnetProjet5.Models.Services
{
    public interface IRepairService
    {
        Task<List<Repair>> GetRepairsByVehicleAsync(int vehicleId);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<List<RepairViewModel>> GetRepairsByVehicleIdAsync(int vehicleId);
        Task AddRepairAsync(Repair repair);
        Task DeleteRepairsByVehicleAsync(int vehicleId);
        Task<Repair> GetRepairByIdAsync(int id);
        Task UpdateRepairAsync(Repair repair);
        Task<bool> RepairExistsAsync(int id);
    }
}
