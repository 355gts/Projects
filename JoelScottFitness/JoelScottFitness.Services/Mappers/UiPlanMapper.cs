using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class UiPlanMapper : ITypeMapper<Plan, UiPlanViewModel>
    {
        PlanMapper planMapper = new PlanMapper();

        public UiPlanViewModel Map(Plan fromObject, UiPlanViewModel toObject = null)
        {
            var plan = toObject ?? new UiPlanViewModel();

            planMapper.Map(fromObject, plan as PlanViewModel);

            return plan;
        }
    }
}
