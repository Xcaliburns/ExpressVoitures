using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.Services;

namespace DotnetProjet5.Services
{
    public class RepairService : IRepairService
    {
        private readonly ApplicationDbContext _context;

        public RepairService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Repair>> GetRepairsByVehicleAsync(string codeVin)
        {
            return await _context.Repairs.Where(r => r.CodeVin == codeVin).ToListAsync();
        }
    }
}
