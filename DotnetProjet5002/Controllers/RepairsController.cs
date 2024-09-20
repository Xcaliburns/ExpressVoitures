using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.ViewModels;
using DotnetProjet5.Models;
using Microsoft.AspNetCore.Authorization;
using DotnetProjet5.Models.Entities;

namespace DotnetProjet5.Controllers
{

    [Authorize(Roles = "Admin,Developer")]
    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repairs
        [Authorize]
        public async Task<IActionResult> Index(int vehicleId)
        {
            var repairs = await _context.Repairs
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
            ViewBag.vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == vehicleId); // Passer le véhicule à la vue
            ViewBag.VehicleId = vehicleId; // Passer l'ID du véhicule à la vue
            
            return View(repairs);
        }

      
        // GET: Repairs/Create     
        [HttpGet]
        public IActionResult Create(int id)
        {
            var vehicle = _context.Vehicle.FirstOrDefault(v => v.VehicleId== id);
            if (vehicle == null)
            {
                return NotFound();
            }
           
            var model = new RepairViewModel
            {
                VehicleId = vehicle.VehicleId,
                Vehicle = vehicle
            };
            return View(model);
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(RepairViewModel repairViewModel)
        {
            if (ModelState.IsValid)
            {
                var repair = new Repair
                {
                    Description = repairViewModel.Description,
                    RepairCost = repairViewModel.RepairCost,
                    VehicleId = repairViewModel.VehicleId,
                    Vehicle = repairViewModel.Vehicle
                };

                // Ajoutez la réparation à la base de données
                _context.Add(repair);
                await _context.SaveChangesAsync();

                // Récupérez le véhicule associé en utilisant repairViewModel.VehicleId
                var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == repairViewModel.VehicleId);
                if (vehicle != null)
                {
                    // Mettez à jour le prix de vente du véhicule
                    vehicle.SellPrice += repair.RepairCost;

                    // Enregistrez le véhicule mis à jour dans la base de données
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index), new { vehicleId = repairViewModel.VehicleId });
            }
            return View(repairViewModel);
        }

        // GET: Repairs/Edit/5
       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                // Gérer le cas où la table est vide
                return NotFound("Aucun enregistrement de réparation trouvé.");
            }
            var repairViewModel = RepairViewModel.ToViewModel(repair);
            return View(repairViewModel);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,Description,RepairCost,VehicleId")] RepairViewModel repairViewModel)
        {
            if (id != repairViewModel.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Retrieve the old repair without tracking
                        var oldRepair = await _context.Repairs.AsNoTracking().FirstOrDefaultAsync(r => r.RepairId == id);
                        if (oldRepair == null)
                        {
                            return NotFound();
                        }

                        // Retrieve the associated vehicle
                        var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == oldRepair.VehicleId);
                        if (vehicle != null)
                        {
                            // Subtract the old repair cost from the vehicle's sell price
                            vehicle.SellPrice -= oldRepair.RepairCost;

                            // Add the new repair cost to the vehicle's sell price
                            vehicle.SellPrice += repairViewModel.RepairCost;

                            // Update the vehicle in the database
                            _context.Update(vehicle);
                        }

                        // Ensure the VehicleId is valid
                        var newVehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == repairViewModel.VehicleId);
                        if (newVehicle == null)
                        {
                            return NotFound("Vehicle not found.");
                        }

                        // Update the repair
                        var updatedRepair = RepairViewModel.ToEntity(repairViewModel);
                        _context.Update(updatedRepair);
                        await _context.SaveChangesAsync();

                        // Commit transaction
                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        await transaction.RollbackAsync();
                        if (!RepairExists(repairViewModel.RepairId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                // Redirigez vers l'action Index avec le vehicleId
                return RedirectToAction(nameof(Index), new { vehicleId = repairViewModel.VehicleId });
            }
            return View(repairViewModel);
        }

        // GET: Repairs/Delete/5
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.Vehicle) // Include the related Vehicle entity
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            var repairViewModel = RepairViewModel.ToViewModel(repair);
            return View(repairViewModel);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                return NotFound("Repair not found.");
            }

            // Récupérer le véhicule associé
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleId == repair.VehicleId);
            if (vehicle != null)
            {
                // Réduire le prix de vente du véhicule du coût de la réparation
                vehicle.SellPrice -= repair.RepairCost;

                // Enregistrer le véhicule mis à jour dans la base de données
                _context.Update(vehicle);
            }

            // Supprimer la réparation de la base de données
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { vehicleId = repair.VehicleId });
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
