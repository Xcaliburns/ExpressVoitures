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
using Microsoft.AspNetCore.Authorization;

namespace DotnetProjet5.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVehicleService _vehicleService;
        private readonly IRepairService _repairService;
        private readonly IFileUploadHelper _fileUploadHelper;

        public VehiclesController(ApplicationDbContext context, IVehicleService vehicleService, IRepairService repairService, IFileUploadHelper fileUploadHelper)
        {
            _context = context;
            _vehicleService = vehicleService;
            _repairService = repairService;
            _fileUploadHelper = fileUploadHelper;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var vehicleViewModels = await _vehicleService.GetAllVehiclesAsync();
            return View(vehicleViewModels);
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleViewModel = await _vehicleService.GetVehicleByCodeVinAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }

            return View(vehicleViewModel);
        }

        // GET: Vehicles/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new VehicleViewModel());
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel, IFormFile imageFile)//verifier pourquoi 2 vqriables
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateVehicleAsync(vehicleViewModel, imageFile);//verifier pouquoi 2 variables
                return RedirectToAction(nameof(Index));
            }



            return View(vehicleViewModel);
        }

        // GET: Vehicles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleViewModel = await _vehicleService.GetVehicleByCodeVinAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }
            return View(vehicleViewModel);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(string id, VehicleViewModel vehicleViewModel, IFormFile imageFile)
        {
            if (id != vehicleViewModel.CodeVin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        // Assuming the image URL is set by another service before calling UpdateVehicleAsync
                        vehicleViewModel.ImageUrl = await SaveImageFileAsync(imageFile);
                    }

                    await _vehicleService.UpdateVehicleAsync(vehicleViewModel, imageFile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _vehicleService.VehicleExistsAsync(vehicleViewModel.CodeVin))
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
            return View(vehicleViewModel);
        }

        // GET: Vehicles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleViewModel = await _vehicleService.GetVehicleByCodeVinAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }

            return View(vehicleViewModel);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string CodeVin)
        {  //TODO: remember to delete the image file
            await _vehicleService.DeleteVehicleAsync(CodeVin);
            return RedirectToAction(nameof(Index));
        }
        // TODO A deplacer
        private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return string.Empty;
            }

            var filePath = Path.Combine("wwwroot/images", imageFile.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
