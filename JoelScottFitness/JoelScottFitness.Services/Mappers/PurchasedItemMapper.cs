using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PurchasedItemMapper : ITypeMapper<PurchasedItem, PurchasedHistoryItemViewModel>, ITypeMapper<PurchasedHistoryItemViewModel, PurchasedItem>
    {
        public PurchasedHistoryItemViewModel Map(PurchasedItem fromObject, PurchasedHistoryItemViewModel toObject = null)
        {
            var item = toObject ?? new PurchasedHistoryItemViewModel();
            
            
            item.Description = fromObject.Item.Description;
            item.Id = fromObject.Id;
            item.ItemId = fromObject.ItemId;
            item.ItemType = fromObject.Item.ItemType;
            item.Price = fromObject.Item.Price;
            item.Quantity = fromObject.Quantity;
            item.RequiresAction = fromObject.RequiresAction;
            item.PlanPath = fromObject.PlanPath;
            item.PlanOptionId = fromObject.Item.Id;

            return item;
        }

        public PurchasedItem Map(PurchasedHistoryItemViewModel fromObject, PurchasedItem toObject = null)
        {
            var item = toObject ?? new PurchasedItem();
            
            item.Id = fromObject.Id;
            item.ItemId = fromObject.ItemId;
            item.Quantity = fromObject.Quantity;

            return item;
        }
    }
}
