using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace DotnetProjet5.Models.Entities
{
    public class Vehicle
    {
        [Key]
        public string CodeVin { get; set; }
        public DateTime Year { get; set; }
        public DateTime PurchaseDate { get; set; }
        public float PurchasePrice { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public required string Description { get; set; }
        public bool Availability { get; set; } = false;
        public string ImageUrl { get; set; }

        public DateTime? AvailabilityDate { get; set; }
        public float SellPrice { get; set; }
        public bool Selled { get; set; }
    }

}
