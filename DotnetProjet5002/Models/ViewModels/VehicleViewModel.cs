using System;
using System.Collections.Generic;
using DotnetProjet5.Models;
using DotnetProjet5.Models.Entities;
using System.ComponentModel.DataAnnotations;
using DotnetProjet5.ValidationAttributes;

namespace DotnetProjet5.ViewModels
{
    public class VehicleViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Code VIN")]
        public string CodeVin { get; set; }

        [Required]
        [Display(Name = "Année")]
        [MinYearValidation(1993, ErrorMessage = "L'année doit être supérieure ou égale à 1993.")]
        [MaxYearValidation(ErrorMessage = "L'année doit être inférieure ou égale à l'année en cours.")]
        public DateTime Year { get; set; }

        [Required]
        [Display(Name = "Date d'achat")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Display(Name = "Prix d'achat")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Merci d'entrer un prix valide.")]
        public float PurchasePrice { get; set; }

        [Required]
        [Display(Name = "Marque")]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Modèle")]
        public string Model { get; set; }

        [Display(Name = "Finition")]
        public string Finish { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Prix de vente")]
        public float SellPrice { get; set; }

        [Display(Name = "Disponibilité")]
        public bool Availability { get; set; }

        [RequiredForCreate(ErrorMessage = "Veuillez ajouter une image.")]
        [Display(Name = "Visuel")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Chemin de l'image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Date de disponibilité")]
        public DateTime? AvailabilityDate { get; set; }

        [Display(Name = "Vendu")]
        public bool Selled { get; set; }
    }
}
