using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotnetProjet5.Models.Entities;

namespace DotnetProjet5.Models
{
    public class Sale
    {

        public int SaleId { get; set; }

        [ForeignKey("Vehicle")]
        public string codeVin { get; set; }
        public Vehicle Vehicle { get; set; }

        public DateTime SaleDate { get; set; }
    }
}
