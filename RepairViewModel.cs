using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.Models.ViewModels
{
    public class RepairViewModel
    {
        [Required(ErrorMessage = "The RepairId field is required.")]
        public int RepairId { get; set; }

        [Required(ErrorMessage = "The CodeVin field is required.")]
        public string CodeVin { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The RepairCost field is required.")]
        public float RepairCost { get; set; }
    }
}
