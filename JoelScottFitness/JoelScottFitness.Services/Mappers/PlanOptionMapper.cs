using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PlanOptionMapper : ITypeMapper<PlanOption, PlanOptionViewModel>, ITypeMapper<PlanOptionViewModel, PlanOption>
    {
        ItemMapper itemMapper = new ItemMapper();

        public PlanOption Map(PlanOptionViewModel fromObject, PlanOption toObject = null)
        {
            var planOption = toObject ?? new PlanOption();

            itemMapper.Map(fromObject, planOption);

            planOption.PlanId = fromObject.PlanId;
            planOption.Duration = fromObject.Duration;

            //if (fromObject.Plan != null)
            //{
            //    var plan = new Plan();
            //    plan.BannerHeader = fromObject.Plan.BannerHeader;
            //    plan.CreatedDate = fromObject.Plan.CreatedDate;
            //    plan.Description = fromObject.Plan.Description;
            //    plan.Id = fromObject.Plan.Id;
            //    plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
            //    plan.ModifiedDate = fromObject.Plan.ModifiedDate;
            //    plan.Name = fromObject.Plan.Name;
            //    plan.TargetGender = fromObject.Plan.TargetGender;
            //    plan.Active = fromObject.Plan.Active;
            //    plan.ModifiedDate = fromObject.Plan.ModifiedDate;
            //    plan.BannerColour = fromObject.Plan.BannerColour;

            //    planOption.Plan = plan;
            //}

            return planOption;
        }

        public PlanOptionViewModel Map(PlanOption fromObject, PlanOptionViewModel toObject = null)
        {
            var planOption = toObject ?? new PlanOptionViewModel();

            itemMapper.Map(fromObject, planOption);

            planOption.PlanId = fromObject.PlanId;
            planOption.Duration = fromObject.Duration;

            //if (fromObject.Plan != null)
            //{
            //    var plan = new PlanViewModel();
            //    plan.BannerHeader = fromObject.Plan.BannerHeader;
            //    plan.CreatedDate = fromObject.Plan.CreatedDate;
            //    plan.Description = fromObject.Plan.Description;
            //    plan.Id = fromObject.Plan.Id;
            //    plan.ImagePathLarge = fromObject.Plan.ImagePathLarge;
            //    plan.ModifiedDate = fromObject.Plan.ModifiedDate;
            //    plan.Name = fromObject.Plan.Name;
            //    plan.TargetGender = fromObject.Plan.TargetGender;
            //    plan.Active = fromObject.Plan.Active;
            //    plan.ModifiedDate = fromObject.Plan.ModifiedDate;
            //    plan.BannerColour = fromObject.Plan.BannerColour;

            //    planOption.Plan = plan;
            //}

            return planOption;
        }
    }
}
