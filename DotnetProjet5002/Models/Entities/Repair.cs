using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DotnetProjet5.Models.Entities;

namespace DotnetProjet5.Models
{
    public class Repair
    {

        public int RepairId { get; set; }

        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        
        public string Description { get; set; }
        public float RepairCost { get; set; }

        
        public Vehicle Vehicle { get; set; }
    }
}
