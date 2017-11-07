using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreatePlanOptionMapper : ITypeMapper<CreatePlanOptionViewModel, PlanOption>
    {
        public PlanOption Map(CreatePlanOptionViewModel fromObject, PlanOption toObject = null)
        {
            var planOption = toObject ?? new PlanOption();

            planOption.Description = fromObject.Description;
            planOption.Duration = fromObject.Duration;
            planOption.ItemType = fromObject.ItemType;
            planOption.Price = fromObject.Price;

            return planOption;
        }
    }
}
