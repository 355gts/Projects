using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class HallOfFameMapper : ITypeMapper<PurchasedItem, HallOfFameViewModel>
    {
        public HallOfFameViewModel Map(PurchasedItem fromObject, HallOfFameViewModel toObject = null)
        {
            var hallOfFameEntry = toObject ?? new HallOfFameViewModel();

            hallOfFameEntry.PurchasedItemId = fromObject.Id;
            hallOfFameEntry.AfterImagePath = fromObject.AfterImage;
            hallOfFameEntry.BeforeImagePath = fromObject.BeforeImage;
            hallOfFameEntry.Comment = fromObject.Comment;
            hallOfFameEntry.Name = $"{fromObject.Purchase.Customer.Firstname} {fromObject.Purchase.Customer.Surname}";
            hallOfFameEntry.PlanDescription = fromObject.Item.Description;
            hallOfFameEntry.ItemId = fromObject.ItemId;
            hallOfFameEntry.Enabled = fromObject.HallOfFameEnabled;

            return hallOfFameEntry;
        }
    }
}
