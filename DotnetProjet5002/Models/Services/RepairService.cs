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

        public RepairService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Repair>> GetRepairsByVehicleAsync(int vehicleId)
        {
            return await _context.Repairs.Where(r => r.VehicleId == vehicleId).ToListAsync();
        }

        public async Task DeleteRepairsByVehicleAsync(int vehicleId)
        {
            var repairs = await GetRepairsByVehicleAsync(vehicleId);
            _context.Repairs.RemoveRange(repairs);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RepairViewModel>> GetRepairsByVehicleIdAsync(int vehicleId)
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

        public async Task AddRepairAsync(Repair repair) 
        {
            _context.Repairs.Add(repair);
            await _context.SaveChangesAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {           
            
                return await _context.Vehicle.FindAsync(vehicleId);

        }

        public async Task<Repair> GetRepairByIdAsync(int id)
        {
            return await _context.Repairs
                .Include(r => r.Vehicle) 
                .FirstOrDefaultAsync(r => r.RepairId == id);
        }
        public async Task UpdateRepairAsync(Repair repair)
        {
            _context.Repairs.Update(repair);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RepairExistsAsync(int id)
        {
            return await _context.Repairs.AnyAsync(e => e.RepairId == id);
        }
    }
}
