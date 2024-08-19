using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotnetProjet5.Data;
using DotnetProjet5.Models.Entities;
using DotnetProjet5.ViewModels;

using DotnetProjet5.Models.Services;

namespace DotnetProjet5.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleService _vehicleService;
        private readonly IRepairService _repairService;

        public VehiclesController(ApplicationDbContext context, IVehicleService vehicleService, IRepairService repairService)
        {
            _context = context;
            _vehicleService = vehicleService;
            _repairService = repairService;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicle.ToListAsync();
            var vehicleViewModels = vehicles.Select(vehicle => new VehicleViewModel
            {
                CodeVin = vehicle.CodeVin,
                Year = vehicle.Year,
                PurchaseDate = vehicle.PurchaseDate,
                PurchasePrice = vehicle.PurchasePrice,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Finish = vehicle.Finish,
                Description = vehicle.Description,
                Availability = vehicle.Availability,
                ImageUrl = vehicle.ImageUrl,
                AvailabilityDate = vehicle.AvailabilityDate,
                Selled = vehicle.Selled,
                SellPrice = vehicle.SellPrice
                
            }).ToList();
            // Log the SellPrice values for debugging
            foreach (var vehicle in vehicleViewModels)
            {
                Console.WriteLine($"Vehicle {vehicle.CodeVin} - SellPrice: {vehicle.SellPrice}");
            }

            return View(vehicleViewModels);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.CodeVin == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View(new VehicleViewModel());
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel )
        {
            if (ModelState.IsValid)
            {

                // Calculate the total repair cost
                float totalRepairCost = 0;
                if (_repairService != null)
                {
                    var repairs = await _repairService.GetRepairsByVehicleAsync(vehicleViewModel.CodeVin);
                    totalRepairCost = repairs.Sum(repair => repair.RepairCost);
                }

                // Calculate the sell price
                float sellPrice = vehicleViewModel.PurchasePrice + totalRepairCost + 500;
                var vehicle = new Vehicle
                {
                    CodeVin = vehicleViewModel.CodeVin,
                    Year = vehicleViewModel.Year,
                    PurchaseDate = vehicleViewModel.PurchaseDate,
                    PurchasePrice = vehicleViewModel.PurchasePrice,
                    Brand = vehicleViewModel.Brand,
                    Model = vehicleViewModel.Model,
                    Finish = vehicleViewModel.Finish,
                    Description = vehicleViewModel.Description,
                    Availability = vehicleViewModel.Availability,
                    ImageUrl = vehicleViewModel.ImageUrl,
                    AvailabilityDate = vehicleViewModel.AvailabilityDate,
                    Selled = vehicleViewModel.Selled,
                    SellPrice = sellPrice,
                };

                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Log ModelState errors
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(vehicleViewModel);
        }

    

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CodeVin,Year,PurchaseDate,PurchasePrice,Brand,Model,Finish,Description,Availability,ImageUrl,AvailabilityDate,selled")] Vehicle vehicle)
        {
            if (id != vehicle.CodeVin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.CodeVin))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.CodeVin == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.CodeVin == id);
        }
    }
}
