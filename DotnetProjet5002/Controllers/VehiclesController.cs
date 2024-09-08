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
using System.Collections.Immutable;

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
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin,Developer")]
        public IActionResult Create()
        {
            return View(new VehicleViewModel());
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel, IFormFile imageFile)//verifier pourquoi 2 vqriables
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateVehicleAsync(vehicleViewModel);//verifier pouquoi 2 variables

                return RedirectToAction(nameof(CreateConfirmed));
            }



            return View(vehicleViewModel);
        }

        // GET: Vehicles/Edit/5
        [Authorize(Roles = "Admin,Developer")]
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
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Edit(string id, VehicleViewModel vehicleViewModel)
        {
            if (id != vehicleViewModel.CodeVin)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {


                if (vehicleViewModel.ImageFile != null)
                {
                    
                    // Retrieve the existing ImageUrl from the database
                    var existingImageUrl = _context.Vehicle
                        .Where(v => v.CodeVin == vehicleViewModel.CodeVin)
                        .Select(v => v.ImageUrl)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(existingImageUrl))
                    {
                        var oldImagePath = Path.Combine("wwwroot/images", Path.GetFileName(existingImageUrl));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    // Save the new image file and update the ImageUrl
                    vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(vehicleViewModel.ImageFile);
                }
                else
                {
                    // Retrieve the existing ImageUrl from the database
                    vehicleViewModel.ImageUrl = _context.Vehicle
                        .Where(v => v.CodeVin == vehicleViewModel.CodeVin)
                        .Select(v => v.ImageUrl)
                        .FirstOrDefault();
                }
                await _vehicleService.UpdateVehicleAsync(vehicleViewModel);
            }




            return RedirectToAction(nameof(Index));

            return View(vehicleViewModel);
        }

        // GET: Vehicles/Delete/5
        [Authorize(Roles = "Admin,Developer")]
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
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> DeleteConfirmed(string CodeVin)
        {  
            await _vehicleService.DeleteVehicleAsync(CodeVin);
            // Redirect to Home/Index
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Developer")]
        public IActionResult CreateConfirmed()
        {
            return View();
        }

        // TODO A deplacer
        //private async Task<string> SaveImageFileAsync(IFormFile imageFile)
        //{
        //    if (imageFile == null)
        //    {
        //        return string.Empty;
        //    }

        //    var filePath = Path.Combine("wwwroot/images", imageFile.FileName);
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await imageFile.CopyToAsync(stream);
        //    }

        //    return filePath;
        //}
    }
}
