using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreatePlanMapper : ITypeMapper<CreatePlanViewModel, Plan>
    {
        CreatePlanOptionMapper planOptionMapper = new CreatePlanOptionMapper();

        public Plan Map(CreatePlanViewModel fromObject, Plan toObject = null)
        {
            var plan = toObject ?? new Plan();

            plan.BannerHeader = fromObject.BannerHeader;
            plan.CreatedDate = DateTime.UtcNow;
            plan.Description = fromObject.Description;
            plan.ImagePathLarge = fromObject.ImagePathLarge;
            plan.ImagePathMedium = fromObject.ImagePathMedium;
            plan.Name = fromObject.Name;
            plan.TargetGender = fromObject.TargetGender;
            plan.Active = false;
            plan.BannerColour = fromObject.BannerColour;

            var planOptions = new List<PlanOption>();

            foreach (var planoption in fromObject.Options)
            {
                planOptions.Add(planOptionMapper.Map(planoption));
            }

            plan.Options = planOptions;

            return plan;
        }
    }
}
