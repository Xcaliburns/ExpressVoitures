using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace DotnetProjet5.Models
{
    public class Vehicle
    {
        [Key]
        public string CodeVin { get; set; }
        public DateTime Year { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Finish { get; set; }
        public bool Availability { get; set; }
        public DateTime? AvailabilityDate { get; set; }
    }

}
