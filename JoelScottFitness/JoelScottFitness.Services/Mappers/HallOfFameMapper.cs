using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class HallOfFameMapper : ITypeMapper<CustomerPlan, HallOfFameViewModel>
    {
        public HallOfFameViewModel Map(CustomerPlan fromObject, HallOfFameViewModel toObject = null)
        {
            var hallOfFameEntry = toObject ?? new HallOfFameViewModel();

            hallOfFameEntry.AfterImagePath = fromObject.AfterImage;
            hallOfFameEntry.BeforeImagePath = fromObject.BeforeImage;
            hallOfFameEntry.Comment = fromObject.Comment;
            hallOfFameEntry.Enabled = fromObject.HallOfFameEnabled;
            hallOfFameEntry.Name = $"{fromObject.Customer?.Firstname} {fromObject.Customer?.Surname}";
            hallOfFameEntry.OrderId = fromObject.OrderId;
            hallOfFameEntry.PlanDescription = fromObject.Item?.Description;
            hallOfFameEntry.PlanName = fromObject.Item?.Name;
            hallOfFameEntry.PlanId = fromObject.Id;

            return hallOfFameEntry;
        }
    }
}
