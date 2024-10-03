using DotnetProjet5.Models.Entities;
using DotnetProjet5.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DotnetProjet5.Models.Mappers
{
    public class RepairMapper : IRepairMapper
    {
        private readonly ILogger<RepairMapper> _logger;

        public RepairMapper(ILogger<RepairMapper> logger)
        {
            _logger = logger;
        }

        public RepairViewModel ToViewModel(Repair repair)
        {
            try
            {
                return new RepairViewModel
                {
                    RepairId = repair.RepairId,
                    VehicleId = repair.VehicleId,
                    Description = repair.Description,
                    RepairCost = repair.RepairCost
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du mapping de Repair vers RepairViewModel");
                throw new MappingException("Erreur lors du mapping de Repair vers RepairViewModel", ex);
            }
        }

        public IEnumerable<RepairViewModel> ToViewModel(IEnumerable<Repair> repairs)
        {
            try
            {
                return repairs.Select(r => ToViewModel(r)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du mapping de la liste de Repairs vers la liste de RepairViewModels");
                throw new MappingException("Erreur lors du mapping de la liste de Repairs vers la liste de RepairViewModels", ex);
            }
        }

        public Repair ToEntity(RepairViewModel viewModel)
        {
            try
            {
                return new Repair
                {
                    RepairId = viewModel.RepairId,
                    VehicleId = viewModel.VehicleId,
                    Description = viewModel.Description,
                    RepairCost = viewModel.RepairCost
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du mapping de RepairViewModel vers Repair");
                throw new MappingException("Erreur lors du mapping de RepairViewModel vers Repair", ex);
            }
        }
    }

    public class MappingException : Exception
    {
        public MappingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
