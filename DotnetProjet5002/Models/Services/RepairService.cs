using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.Services;
using DotnetProjet5.Models.ViewModels;

namespace DotnetProjet5.Services
{
    public class RepairService : IRepairService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RepairService> _logger;

        public RepairService(ApplicationDbContext context, ILogger<RepairService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Repair>> GetRepairsByVehicleAsync(int vehicleId)
        {
            try
            {
                return await _context.Repairs.Where(r => r.VehicleId == vehicleId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la récupération des réparations pour le véhicule ID {vehicleId} : {ex.Message}");
                return new List<Repair>();
            }
        }

        public async Task DeleteRepairsByVehicleAsync(int vehicleId)
        {
            try
            {
                var repairs = await GetRepairsByVehicleAsync(vehicleId);
                _context.Repairs.RemoveRange(repairs);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la suppression des réparations pour le véhicule ID {vehicleId} : {ex.Message}");
            }
        }

        public async Task DeleteRepairByIdAsync(int repairId)
        {
            try
            {
                var repair = await GetRepairByIdAsync(repairId);
                _context.Repairs.Remove(repair);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la suppression de la réparation ID {repairId} : {ex.Message}");
            }
        }

        public async Task<List<RepairViewModel>> GetRepairsByVehicleIdAsync(int vehicleId)
        {
            try
            {
                return await _context.Repairs
                    .Where(r => r.VehicleId == vehicleId)
                    .Select(r => new RepairViewModel
                    {
                        RepairId = r.RepairId,
                        Description = r.Description,
                        RepairCost = r.RepairCost,
                        VehicleId = r.VehicleId,
                        Vehicle = r.Vehicle
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la récupération des réparations pour le véhicule ID {vehicleId} : {ex.Message}");
                return new List<RepairViewModel>();
            }
        }

        public async Task AddRepairAsync(Repair repair)
        {
            try
            {
                _context.Repairs.Add(repair);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de l'ajout de la réparation : {ex.Message}");
            }
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            try
            {
                return await _context.Vehicle.FindAsync(vehicleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la récupération du véhicule ID {vehicleId} : {ex.Message}");
                return null;
            }
        }

        public async Task<Repair> GetRepairByIdAsync(int id)
        {
            try
            {
                return await _context.Repairs
                    .FirstOrDefaultAsync(r => r.RepairId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la récupération de la réparation ID {id} : {ex.Message}");
                return null;
            }
        }

        public async Task UpdateRepairAsync(Repair repair)
        {
            try
            {
                _context.Repairs.Update(repair);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la mise à jour de la réparation ID {repair.RepairId} : {ex.Message}");
            }
        }

        public async Task<bool> RepairExistsAsync(int id)
        {
            try
            {
                return await _context.Repairs.AnyAsync(e => e.RepairId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Une erreur s'est produite lors de la vérification de l'existence de la réparation ID {id} : {ex.Message}");
                return false;
            }
        }
    }
}
