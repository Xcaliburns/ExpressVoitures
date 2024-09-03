using DotnetProjet5.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.Models.ViewModels
{
    public class RepairViewModel
    {
        public int RepairId { get; set; }

        [Required(ErrorMessage ="le code VIN est obligatoire")]
        public string CodeVin { get; set; }  // Initialize to avoid nullability issues
        [Required(ErrorMessage = "la description est obligatoire")]
        public string Description { get; set; } = string.Empty; // Initialize to avoid nullability issues
        [Required(ErrorMessage = "le coût de la réparation est obligatoire")]
        public float RepairCost { get; set; }

        public static RepairViewModel ToViewModel(Repair repair)
        {
            return new RepairViewModel
            {
                RepairId = repair.RepairId,
                CodeVin = repair.CodeVin,
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
                CodeVin = viewModel.CodeVin,
                Description = viewModel.Description,
                RepairCost = viewModel.RepairCost
            };
        }

      
    }
}
