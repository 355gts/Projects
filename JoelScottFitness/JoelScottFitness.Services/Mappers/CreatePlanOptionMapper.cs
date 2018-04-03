using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreatePlanOptionMapper : ITypeMapper<CreatePlanOptionViewModel, PlanOption>
    {
        CreateItemMapper itemMapper = new CreateItemMapper();

        public PlanOption Map(CreatePlanOptionViewModel fromObject, PlanOption toObject = null)
        {
            var planOption = toObject ?? new PlanOption();

            itemMapper.Map(fromObject, planOption);

            planOption.Duration = fromObject.Duration;

            return planOption;
        }
    }
}
