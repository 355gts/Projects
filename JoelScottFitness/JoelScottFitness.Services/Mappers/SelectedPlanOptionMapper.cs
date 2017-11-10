using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class SelectedPlanOptionMapper : ITypeMapper<PlanOption, SelectedPlanOptionViewModel>
    {
        PlanOptionMapper planOptionMapper = new PlanOptionMapper();
        
        public SelectedPlanOptionViewModel Map(PlanOption fromObject, SelectedPlanOptionViewModel toObject = null)
        {
            var planOption = toObject ?? new SelectedPlanOptionViewModel();

            return (SelectedPlanOptionViewModel)planOptionMapper.Map(fromObject, planOption as PlanOptionViewModel);
        }
    }
}
