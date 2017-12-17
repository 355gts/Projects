using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PlanMapper : ITypeMapper<Plan, PlanViewModel>, ITypeMapper<PlanViewModel, Plan>
    {
        PlanOptionMapper planOptionMapper = new PlanOptionMapper();

        public PlanViewModel Map(Plan fromObject, PlanViewModel toObject = null)
        {
            var plan = toObject ?? new PlanViewModel();
            
            plan.BannerHeader = fromObject.BannerHeader;
            plan.CreatedDate = fromObject.CreatedDate;
            plan.Description = fromObject.Description;
            plan.Id = fromObject.Id;
            plan.ImagePathLarge = fromObject.ImagePathLarge;
            plan.ImagePathMedium = fromObject.ImagePathMedium;
            plan.ModifiedDate = fromObject.ModifiedDate;
            plan.Name = fromObject.Name;
            plan.TargetGender = fromObject.TargetGender;
            plan.Active = fromObject.Active;
            plan.ModifiedDate = DateTime.UtcNow;
            plan.BannerColour = fromObject.BannerColour;

            var planOptions = new List<PlanOptionViewModel>();

            foreach (var planoption in fromObject.Options)
            {
                planOptions.Add(planOptionMapper.Map(planoption));
            }

            plan.Options = planOptions;

            return plan;
        }

        public Plan Map(PlanViewModel fromObject, Plan toObject = null)
        {
            var plan = toObject ?? new Plan();
            
            plan.BannerHeader = fromObject.BannerHeader;
            plan.CreatedDate = fromObject.CreatedDate;
            plan.Description = fromObject.Description;
            plan.Id = fromObject.Id;
            plan.ImagePathLarge = fromObject.ImagePathLarge;
            plan.ImagePathMedium = fromObject.ImagePathMedium;
            plan.ModifiedDate = fromObject.ModifiedDate;
            plan.Name = fromObject.Name;
            plan.TargetGender = fromObject.TargetGender;
            plan.Active = fromObject.Active;
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
