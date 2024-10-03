using DotnetProjet5.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.Models.ViewModels
{
    public class RepairViewModel
    {
        public int RepairId { get; set; }

        public int VehicleId { get; set; }  

        [Required(ErrorMessage = "la description est obligatoire")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty; 

        [Required(ErrorMessage = "le coût de la réparation est obligatoire")]
        [Range(0.01, float.MaxValue, ErrorMessage = "le coût de la réparation doit être supérieur à 0")]
        [Display(Name = "Prix de la réparation")]
        public float RepairCost { get; set; }

        public Vehicle? Vehicle { get; set; }

        
        public List<RepairViewModel> Repairs { get; set; } = new List<RepairViewModel>();

        public static RepairViewModel ToViewModel(Repair repair)
        {
            return new RepairViewModel
            {
                RepairId = repair.RepairId,
                VehicleId = repair.VehicleId,
                Description = repair.Description,
                RepairCost = repair.RepairCost,
                Vehicle = repair.Vehicle
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
                Vehicle = viewModel.Vehicle
            };
        }
    }
}
