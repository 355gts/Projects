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

            if (fromObject.Plan != null)
            {
                plan.Plan = new Plan();
                plan.Plan.Active = fromObject.Plan.Active;
                plan.Plan.BannerHeader = fromObject.Plan.BannerHeader;
                plan.Plan.CreatedDate = fromObject.Plan.CreatedDate;
                plan.Plan.Description = fromObject.Plan.Description;
                plan.Plan.Id = fromObject.Plan.Id;
                plan.Plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
                plan.Plan.ImagePathMedium = fromObject.Plan.ImagePathMedium;
                plan.Plan.ModifiedDate = fromObject.Plan.ModifiedDate;
                plan.Plan.Name = fromObject.Plan.Name;
                plan.Plan.TargetGender = fromObject.Plan.TargetGender;
            }

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

            if (fromObject.Plan != null)
            {
                plan.Plan = new PlanViewModel();
                plan.Plan.Active = fromObject.Plan.Active;
                plan.Plan.BannerHeader = fromObject.Plan.BannerHeader;
                plan.Plan.CreatedDate = fromObject.Plan.CreatedDate;
                plan.Plan.Description = fromObject.Plan.Description;
                plan.Plan.Id = fromObject.Plan.Id;
                plan.Plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
                plan.Plan.ImagePathMedium = fromObject.Plan.ImagePathMedium;
                plan.Plan.ModifiedDate = fromObject.Plan.ModifiedDate;
                plan.Plan.Name = fromObject.Plan.Name;
                plan.Plan.TargetGender = fromObject.Plan.TargetGender;
            }

            return plan;
        }
    }
}
