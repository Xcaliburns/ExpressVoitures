using DotnetProjet5.Models.Entities;

namespace DotnetProjet5.Models.Services
{
    public interface IRepairService
    {
        Task<List<Repair>> GetRepairsByVehicleAsync(int vehicleId);
        Task DeleteRepairsByVehicleAsync(int vehicleId);
    }
}
