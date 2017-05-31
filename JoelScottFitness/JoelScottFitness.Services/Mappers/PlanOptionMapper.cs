using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PlanOptionMapper : ITypeMapper<PlanOption, PlanOptionViewModel>, ITypeMapper<PlanOptionViewModel, PlanOption>
    {
        public PlanOption Map(PlanOptionViewModel fromObject, PlanOption toObject = null)
        {
            var plan = toObject ?? new PlanOption();

            plan.ActiveFrom = fromObject.ActiveFrom;
            plan.ActiveTo = fromObject.ActiveTo;
            plan.Description = fromObject.Description;
            plan.Duration = fromObject.Duration;
            plan.Id = fromObject.Id;
            plan.ItemType = fromObject.ItemType;
            plan.PlanId = fromObject.PlanId;
            plan.Price = fromObject.Price;

            return plan;
        }

        public PlanOptionViewModel Map(PlanOption fromObject, PlanOptionViewModel toObject = null)
        {
            var plan = toObject ?? new PlanOptionViewModel();

            plan.ActiveFrom = fromObject.ActiveFrom;
            plan.ActiveTo = fromObject.ActiveTo;
            plan.Description = fromObject.Description;
            plan.Duration = fromObject.Duration;
            plan.Id = fromObject.Id;
            plan.ItemType = fromObject.ItemType;
            plan.PlanId = fromObject.PlanId;
            plan.Price = fromObject.Price;

            return plan;
        }
    }
}
