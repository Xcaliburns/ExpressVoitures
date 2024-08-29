using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace DotnetProjet5.Models.Entities
{
    public class Vehicle
    {
        [Key]
        [Required]
        public string CodeVin { get; set; }

        [Required]
        public DateTime Year { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
      //  [Range(0.1, float.MaxValue, ErrorMessage = "Merci d'entrer un prix valide")]
        public float PurchasePrice { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public string Finish { get; set; }

       
        public string? Description { get; set; }

        public bool Availability { get; set; } = false;

        public string ImageUrl { get; set; }

        public DateTime? AvailabilityDate { get; set; }

     //   [Range(0, float.MaxValue, ErrorMessage = "Merci d'entrer un prix valide")]
        public float SellPrice { get; set; }

        public bool Selled { get; set; }
    }
   

}
