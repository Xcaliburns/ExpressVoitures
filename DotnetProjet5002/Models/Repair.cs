using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetProjet5.Models
{
    public class Repair
    {

        public int RepairId { get; set; }

        [ForeignKey("Vehicle")]
        public string CodeVin { get; set; }
        public Vehicle Vehicle { get; set; }

        public string Description { get; set; }
        public float RepairCost { get; set; }
    }
}
