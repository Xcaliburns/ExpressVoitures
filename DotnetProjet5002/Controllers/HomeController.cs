﻿using DotnetProjet5.Models.Services;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllVehiclesAvailableAsync();
            return View(vehicles);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
