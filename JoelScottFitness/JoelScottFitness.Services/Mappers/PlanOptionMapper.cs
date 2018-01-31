using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PlanOptionMapper : ITypeMapper<PlanOption, PlanOptionViewModel>, ITypeMapper<PlanOptionViewModel, PlanOption>
    {
        public PlanOption Map(PlanOptionViewModel fromObject, PlanOption toObject = null)
        {
            var planOption = toObject ?? new PlanOption();
            
            planOption.Description = fromObject.Description;
            planOption.Duration = fromObject.Duration;
            planOption.Id = fromObject.Id;
            planOption.ItemType = fromObject.ItemType;
            planOption.PlanId = fromObject.PlanId;
            planOption.Price = fromObject.Price;

            if (fromObject.Plan != null)
            {
                planOption.Plan = new Plan();
                planOption.Plan.BannerHeader = fromObject.Plan.BannerHeader;
                planOption.Plan.CreatedDate = fromObject.Plan.CreatedDate;
                planOption.Plan.Description = fromObject.Plan.Description;
                planOption.Plan.Id = fromObject.Plan.Id;
                planOption.Plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
                planOption.Plan.ModifiedDate = fromObject.Plan.ModifiedDate;
                planOption.Plan.Name = fromObject.Plan.Name;
                planOption.Plan.TargetGender = fromObject.Plan.TargetGender;
            }

            return planOption;
        }

        public PlanOptionViewModel Map(PlanOption fromObject, PlanOptionViewModel toObject = null)
        {
            var planOption = toObject ?? new SelectedPlanOptionViewModel();

            planOption.Description = fromObject.Description;
            planOption.Duration = fromObject.Duration;
            planOption.Id = fromObject.Id;
            planOption.ItemType = fromObject.ItemType;
            planOption.PlanId = fromObject.PlanId;
            planOption.Price = fromObject.Price;

            if (fromObject.Plan != null)
            {
                planOption.Plan = new PlanViewModel();
                planOption.Plan.BannerHeader = fromObject.Plan.BannerHeader;
                planOption.Plan.CreatedDate = fromObject.Plan.CreatedDate;
                planOption.Plan.Description = fromObject.Plan.Description;
                planOption.Plan.Id = fromObject.Plan.Id;
                planOption.Plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
                planOption.Plan.ModifiedDate = fromObject.Plan.ModifiedDate;
                planOption.Plan.Name = fromObject.Plan.Name;
                planOption.Plan.TargetGender = fromObject.Plan.TargetGender;
            }

            return planOption;
        }
    }
}
