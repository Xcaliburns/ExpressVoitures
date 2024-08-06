using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetProjet5.Models
{
    public class Purchase
    {
        
        public int PurchaseId { get; set; }

        [ForeignKey("Vehicle")]
        public string CodeVIN { get; set; }
        public Vehicle Vehicle { get; set; }      

        public DateTime PurchaseDate { get; set; }
        public float PurchasePrice { get; set; }
    }
}
