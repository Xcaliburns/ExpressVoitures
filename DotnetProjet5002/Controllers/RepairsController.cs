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
using DotnetProjet5.Models.Services;

namespace DotnetProjet5.Controllers
{

    [Authorize(Roles = "Admin,Developer")]
    public class RepairsController : Controller
    {
        private readonly IRepairService _repairService;
        private readonly IVehicleService _vehicleService;

        public RepairsController(IRepairService repairService, IVehicleService vehicleService)
        {
            _repairService = repairService;
            _vehicleService = vehicleService;
        }



        // GET: Repairs/5

        public async Task<IActionResult> Index(int vehicleId)
        {
            // Utiliser le service de réparation pour obtenir les réparations par ID de véhicule
            var repairs = await _repairService.GetRepairsByVehicleIdAsync(vehicleId);

            // Utiliser le service de véhicule pour obtenir le véhicule par ID
            var vehicle = await _vehicleService.GetVehicleByIdAsync(vehicleId);

            // Passer le véhicule et l'ID du véhicule à la vue
            ViewBag.Vehicle = vehicle;
            ViewBag.VehicleId = vehicleId;
            ViewBag.VehicleYear = vehicle.Year;

            return View(repairs);
        }


        // GET: Repairs/Create     
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var vehicleViewModel = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }

            var vehicle = new Vehicle
            {
                VehicleId = vehicleViewModel.VehicleId,
                CodeVin = vehicleViewModel.CodeVin,
                Year = new DateTime(vehicleViewModel.Year, 1, 1),
                PurchaseDate = vehicleViewModel.PurchaseDate,
                PurchasePrice = vehicleViewModel.PurchasePrice,
                Brand = vehicleViewModel.Brand,
                Model = vehicleViewModel.Model,
                Finish = vehicleViewModel.Finish,
                Description = vehicleViewModel.Description,
                Availability = vehicleViewModel.Availability,
                ImageUrl = vehicleViewModel.ImageUrl,
                AvailabilityDate = vehicleViewModel.AvailabilityDate,
                SellPrice = vehicleViewModel.SellPrice,
                Selled = vehicleViewModel.Selled
            };

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
        public async Task<IActionResult> Create(RepairViewModel repairViewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the associated vehicle using the vehicle service
                var vehicle = await _vehicleService.GetVehicleByIdAsync(repairViewModel.VehicleId);
                if (vehicle == null)
                {
                    return NotFound("Vehicle not found.");
                }

                var repair = new Repair
                {
                    Description = repairViewModel.Description,
                    RepairCost = repairViewModel.RepairCost,
                    VehicleId = repairViewModel.VehicleId,
                    Vehicle = repairViewModel.Vehicle // Ensure Vehicle is not null
                };

                // Use the repair service to add the repair
                await _repairService.AddRepairAsync(repair);

                // Update the vehicle's sell price
                vehicle.SellPrice += repair.RepairCost;

                // Use the vehicle service to update the vehicle
                await _vehicleService.UpdateVehicleAsync(vehicle);

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

            var repair = await _repairService.GetRepairByIdAsync(id.Value);
            if (repair == null)
            {
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
                try
                {
                    // Retrieve the old repair using the repair service
                    var oldRepair = await _repairService.GetRepairByIdAsync(id);
                    if (oldRepair == null)
                    {
                        return NotFound();
                    }

                    // Update the repair
                    var updatedRepair = RepairViewModel.ToEntity(repairViewModel);
                    await _repairService.UpdateRepairAsync(updatedRepair);

                    // Retrieve the associated vehicle using the vehicle service
                    var vehicle = await _vehicleService.GetVehicleByIdAsync(repairViewModel.VehicleId);
                    if (vehicle != null)
                    {
                        // Adjust the vehicle's sell price
                        vehicle.SellPrice += (updatedRepair.RepairCost - oldRepair.RepairCost);

                        // Use the vehicle service to update the vehicle
                        await _vehicleService.UpdateVehicleAsync(vehicle);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _repairService.RepairExistsAsync(repairViewModel.RepairId))
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
                    throw;
                }

                // Redirect to the Index action with the vehicleId
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

            // Utiliser le service de réparation pour obtenir la réparation par ID
            var repair = await _repairService.GetRepairByIdAsync(id.Value);
            if (repair == null)
            {
                return NotFound();
            }

            // Convertir l'entité Repair en RepairViewModel
            var repairViewModel = RepairViewModel.ToViewModel(repair);
            return View(repairViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Utiliser le service de réparation pour obtenir la réparation par ID
            var repair = await _repairService.GetRepairByIdAsync(id);
            if (repair == null)
            {
                return NotFound("Repair not found.");
            }

            // Utiliser le service de véhicule pour obtenir le véhicule associé
            var vehicle = await _vehicleService.GetVehicleByIdAsync(repair.VehicleId);
            if (vehicle != null)
            {
                // Réduire le prix de vente du véhicule du coût de la réparation
                vehicle.SellPrice -= repair.RepairCost;

                // Utiliser le service de véhicule pour mettre à jour le véhicule
                await _vehicleService.UpdateVehicleAsync(vehicle);
            }

            // Utiliser le service de réparation pour supprimer la réparation
            id= repair.VehicleId;
            await _repairService.DeleteRepairsByVehicleAsync(repair.VehicleId);

            return RedirectToAction(nameof(Index), new { vehicleId = repair.VehicleId });
        }


      
    }
}
