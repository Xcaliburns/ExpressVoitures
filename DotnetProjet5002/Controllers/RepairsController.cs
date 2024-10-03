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



        [AllowAnonymous]
        public async Task<IActionResult> Index(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return RedirectToAction("Index", "Home");
            }

           
            var repairs = await _repairService.GetRepairsByVehicleIdAsync(vehicleId.Value);

            
            var vehicle = await _vehicleService.GetVehicleByIdAsync(vehicleId.Value);

            if (vehicle == null)
            {
                return NotFound("Vehicule non trouvé.");
            }

           
            ViewBag.Vehicle = vehicle;
            ViewBag.VehicleId = vehicleId.Value;
            ViewBag.VehicleYear = vehicle.Year;

            return View(repairs);
        }


         
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

        
       
        public async Task<IActionResult> Create(RepairViewModel repairViewModel)
        {
            if (ModelState.IsValid)
            {
               
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
                    Vehicle = repairViewModel.Vehicle 
                };

                
                await _repairService.AddRepairAsync(repair);

                
                vehicle.SellPrice += repair.RepairCost;

                
                await _vehicleService.UpdateVehicleAsync(vehicle);

                return RedirectToAction(nameof(Index), new { vehicleId = repairViewModel.VehicleId });
            }
            return View(repairViewModel);
        }

        

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
                    
                    var oldRepair = await _repairService.GetRepairByIdAsync(id);
                    if (oldRepair == null)
                    {
                        return NotFound();
                    }

                    
                    var updatedRepair = RepairViewModel.ToEntity(repairViewModel);
                    await _repairService.UpdateRepairAsync(updatedRepair);

                    
                    var vehicle = await _vehicleService.GetVehicleByIdAsync(repairViewModel.VehicleId);
                    if (vehicle != null)
                    {
                        
                        vehicle.SellPrice += (updatedRepair.RepairCost - oldRepair.RepairCost);

                        
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

               
                return RedirectToAction(nameof(Index), new { vehicleId = repairViewModel.VehicleId });
            }
            return View(repairViewModel);
        }


       

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var repair = await _repairService.GetRepairByIdAsync(id.Value);
            if (repair == null)
            {
                return NotFound();
            }

            
            var repairViewModel = RepairViewModel.ToViewModel(repair);
            return View(repairViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var repair = await _repairService.GetRepairByIdAsync(id);
            if (repair == null)
            {
                return NotFound("Repair not found.");
            }

            
            var vehicle = await _vehicleService.GetVehicleByIdAsync(repair.VehicleId);
            if (vehicle != null)
            {
                
                vehicle.SellPrice -= repair.RepairCost;

                
                await _vehicleService.UpdateVehicleAsync(vehicle);
            }

            
            
            await _repairService.DeleteRepairByIdAsync(id);

            return RedirectToAction(nameof(Index), new { vehicleId = repair.VehicleId });
        }


      
    }
}
