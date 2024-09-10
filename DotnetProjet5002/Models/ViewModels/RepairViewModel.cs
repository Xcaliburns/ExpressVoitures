using DotnetProjet5.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.Models.ViewModels
{
    public class RepairViewModel
    {
        public int RepairId { get; set; }

        [Required(ErrorMessage = "le code VIN est obligatoire")]
        // Le codeVin est composé de 17 caracteres
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le code VIN doit être composé de 17 caractères.")]
        public string CodeVin { get; set; } = string.Empty; // Initialize to avoid nullability issues

        public int VehicleId { get; set; }  // Initialize to avoid nullability issues

        [Required(ErrorMessage = "la description est obligatoire")]
        public string Description { get; set; } = string.Empty; // Initialize to avoid nullability issues

        [Required(ErrorMessage = "le coût de la réparation est obligatoire")]
        public float RepairCost { get; set; }

        public static RepairViewModel ToViewModel(Repair repair)
        {
            return new RepairViewModel
            {
                RepairId = repair.RepairId,
                VehicleId = repair.VehicleId,
                CodeVin = repair.Vehicle?.CodeVin ?? string.Empty, // Map CodeVin from Vehicle
                Description = repair.Description,
                RepairCost = repair.RepairCost
            };
        }

        public static IEnumerable<RepairViewModel> ToViewModel(IEnumerable<Repair> repairs)
        {
            return repairs.Select(r => ToViewModel(r)).ToList();
        }

        // Méthode de mapping inverse
        public static Repair ToEntity(RepairViewModel viewModel)
        {
            return new Repair
            {
                RepairId = viewModel.RepairId,
                VehicleId = viewModel.VehicleId,
                Description = viewModel.Description,
                RepairCost = viewModel.RepairCost,
                // Note: CodeVin is not directly mapped back to Repair entity as it belongs to Vehicle
            };
        }
    }
}
