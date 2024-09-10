using DotnetProjet5.Models.Entities;
using DotnetProjet5.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace DotnetProjet5.Models.Mappers
{
    public class RepairMapper : IRepairMapper
    {
        public RepairViewModel ToViewModel(Repair repair)
        {
            return new RepairViewModel
            {
                RepairId = repair.RepairId,
                VehicleId = repair.VehicleId,
                Description = repair.Description,
                RepairCost = repair.RepairCost
            };
        }

        public IEnumerable<RepairViewModel> ToViewModel(IEnumerable<Repair> repairs)
        {
            return repairs.Select(r => ToViewModel(r)).ToList();
        }

        public Repair ToEntity(RepairViewModel viewModel)
        {
            return new Repair
            {
                RepairId = viewModel.RepairId,
                VehicleId = viewModel.VehicleId,
                Description = viewModel.Description,
                RepairCost = viewModel.RepairCost
            };
        }
    }
}
