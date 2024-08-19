using System;
using System.Collections.Generic;
using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace DotnetProjet5.ViewModels
{
    public class VehicleViewModel
    {
        [Key]
        [Required]
        
        public string CodeVin { get; set; }

        [Required]
        public DateTime Year { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a valid price")]
        public float PurchasePrice { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }
        public string Finish { get; set; }
        public string Description { get; set; }
        public float SellPrice { get; set; }
        public bool Availability { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public bool Selled { get; set; }

     
        
       // public List<Repair> Repairs { get; set; }
    }
}
