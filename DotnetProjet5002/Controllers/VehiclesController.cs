using Microsoft.AspNetCore.Mvc;
using DotnetProjet5.Data;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return SafeRedirect("/Home/Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var vehicleViewModel = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }

            return View(vehicleViewModel);
        }

        [Authorize(Roles = "Admin,Developer")]
        public IActionResult Create()
        {
            return View(new VehicleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateVehicleAsync(vehicleViewModel);
                return SafeRedirect(nameof(CreateConfirmed));
            }

            return View(vehicleViewModel);
        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleViewModel = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }
            return View(vehicleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Edit(int vehicleid, VehicleViewModel vehicleViewModel)
        {
            if (vehicleid != vehicleViewModel.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vehicleViewModel.ImageFile != null)
                {
                    var existingImageUrl = _context.Vehicle
                        .Where(v => v.VehicleId == vehicleViewModel.VehicleId)
                        .Select(v => v.ImageUrl)
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(existingImageUrl))
                    {
                        var oldImagePath = Path.Combine("wwwroot/images", Path.GetFileName(existingImageUrl));
                        await _fileUploadHelper.DeleteFileIfExistsAsync(oldImagePath);
                    }

                    vehicleViewModel.ImageUrl = await _fileUploadHelper.UploadFileAsync(vehicleViewModel.ImageFile);
                }
                else
                {
                    vehicleViewModel.ImageUrl = _context.Vehicle
                        .Where(v => v.VehicleId == vehicleViewModel.VehicleId)
                        .Select(v => v.ImageUrl)
                        .FirstOrDefault();
                }

                await _vehicleService.UpdateVehicleAsync(vehicleViewModel);
                return SafeRedirect("/Home/Index");
            }

            return View(vehicleViewModel);
        }

        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicleViewModel = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicleViewModel == null)
            {
                return NotFound();
            }

            return View(vehicleViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<IActionResult> DeleteConfirmed(int VehicleId)
        {
            var vehicle = await _context.Vehicle.FindAsync(VehicleId);
            if (vehicle == null)
            {
                return NotFound();
            }

            ViewBag.Brand = vehicle.Brand;
            ViewBag.Model = vehicle.Model;
            ViewBag.Year = vehicle.Year.Year;

            await _vehicleService.DeleteVehicleAsync(VehicleId);

            return View("DeleteConfirmation");
        }

        [Authorize(Roles = "Admin,Developer")]
        public IActionResult DeleteConfirmation()
        {
            ViewBag.Brand = TempData["Brand"];
            ViewBag.Model = TempData["Model"];
            ViewBag.Year = TempData["Year"];
            return View();
        }

        [Authorize(Roles = "Admin,Developer")]
        public IActionResult CreateConfirmed()
        {
            return View();
        }

        // Méthode SafeRedirect pour redirections sécurisées
        private IActionResult SafeRedirect(string url)
        {
            if (IsValidRedirectUrl(url))
            {
                return Redirect(url);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // Méthode pour valider les URL de redirection
        private bool IsValidRedirectUrl(string url)
        {
            // Vérifier si l'URL est un chemin relatif
            return Uri.TryCreate(url, UriKind.Relative, out _);
        }
    }
}
