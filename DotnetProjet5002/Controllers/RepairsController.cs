using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace DotnetProjet5.Controllers
{
    public class RepairsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repairs
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var repairs = await _context.Repairs.ToListAsync();
            var repairViewModels = RepairViewModel.ToViewModel(repairs);
            return View(repairViewModels);
        }

        // GET: Repairs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // GET: Repairs/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create(string id)
        {
            var vehicle = _context.Vehicle.FirstOrDefault(v => v.CodeVin == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["CodeVin"] = id;
            var model = new RepairViewModel
            {
                CodeVin = id // Set the CodeVin from the vehicle
            };
            return View(model);
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create( RepairViewModel repairViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var repair = RepairViewModel.ToEntity(repairViewModel);
                //add the repair to the database
                _context.Add(repair);
                await _context.SaveChangesAsync();

                //fetch the associated vehicle
                var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.CodeVin == repair.CodeVin);
                if (vehicle != null)
                {
                    // Update the vehicle's price
                    vehicle.SellPrice += repair.RepairCost;

                    // Save the updated vehicle back to the database
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(repairViewModel);
        }

        // GET: Repairs/Edit/5
        [Authorize]
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

            return View(repair);
        }


        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("RepairId,CodeVin,Description,RepairCost")] RepairViewModel repair)
        {
            if (id != repair.RepairId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.RepairId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(repair);
        }

        // GET: Repairs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .FirstOrDefaultAsync(m => m.RepairId == id);
            if (repair == null)
            {
                return NotFound();
            }

            return View(repair);
        }

        // POST: Repairs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            if (repair != null)
            {
                _context.Repairs.Remove(repair);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.RepairId == id);
        }
    }
}
