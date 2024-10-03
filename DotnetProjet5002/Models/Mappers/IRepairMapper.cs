using DotnetProjet5.Models.ViewModels;

namespace DotnetProjet5.Models.Mappers
{
    public interface IRepairMapper
    {
        RepairViewModel ToViewModel(Repair repair);
        IEnumerable<RepairViewModel> ToViewModel(IEnumerable<Repair> repairs);
        Repair ToEntity(RepairViewModel viewModel);
    }
}
