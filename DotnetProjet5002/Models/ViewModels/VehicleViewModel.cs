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

        public int VehicleId { get; set; }

        [Required (AllowEmptyStrings = false,ErrorMessage = "le code VIN est requis")]
        
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le code VIN doit être composé de 17 caractères.")]
        [UniqueVIN]
        [Display(Name = "Code VIN")]
        public string CodeVin { get; set; }

        [Required(ErrorMessage ="l'année est est requise")]
        [Display(Name = "Année")]
        [Range(1993, int.MaxValue, ErrorMessage = "L'année doit être comprise entre 1993 et l'année en cours.")]        
        public int Year { get; set; }

        [Required]
        [Display(Name = "Date d'achat")]
        [MaxDate(ErrorMessage = "La date  peut  être aujourd'hui au plus tard.")]
        [PurchaseDateRange(ErrorMessage = "La date d'achat doit être comprise entre aujourd'hui et l'année du véhicule.")]
        public DateTime PurchaseDate { get; set; } = new DateTime(1993, 1, 1);

        [Required(ErrorMessage ="le prix est requis")]
        [Display(Name = "Prix d'achat")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Merci d'entrer un prix valide.")]
        public float PurchasePrice { get; set; }

        [Required (ErrorMessage ="La marque est requise")]
        [Display(Name = "Marque")]
        public string Brand { get; set; }

        [Required(ErrorMessage ="le modèle est requis")]
        [Display(Name = "Modèle")]
        public string Model { get; set; }

        [Required(ErrorMessage ="La finition est requise")]
        [Display(Name = "Finition")]
        public string Finish { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Prix de vente")]
        public float SellPrice { get; set; }

        [RequiredForCreate(ErrorMessage = "Veuillez ajouter une image.")]
        [Display(Name = "Visuel")]
        [MaxFileSize(3 * 1024 * 1024, ErrorMessage = "La taille du fichier ne doit pas dépasser 1 MB.")] // 3 MB limit
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Les fichiers autorisés sont: .jpg, .jpeg, .png.")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Chemin de l'image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Date de disponibilité")]
        public DateTime? AvailabilityDate { get; set; }


        [Display(Name = "Disponibilité")]
        public bool Availability { get; set; }


        [Display(Name = "Vendu")]
        public bool Selled { get; set; }
    }
}
