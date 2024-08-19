using DotnetProjet5.Models.Entities;

namespace DotnetProjet5.Models.Services
{
    public interface IRepairService
    {
       
        Task<List<Repair>> GetRepairsByVehicleAsync(string codeVin);
    }
}
