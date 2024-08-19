using DotnetProjet5.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotnetProjet5.Controllers
{
    public class HomeController : Controller
    {

        private readonly IVehicleService _vehicleService;

        public HomeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return View(vehicles);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
