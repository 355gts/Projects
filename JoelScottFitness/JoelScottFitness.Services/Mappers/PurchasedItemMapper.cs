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
            item.Id = fromObject.Item.Id;
            item.ItemId = fromObject.ItemId;
            item.ItemType = fromObject.Item.ItemType;
            item.Price = fromObject.Item.Price;
            item.Quantity = fromObject.Quantity;

            return item;
        }

        public PurchasedItem Map(PurchasedHistoryItemViewModel fromObject, PurchasedItem toObject = null)
        {
            var item = toObject ?? new PurchasedItem();

            item.Item.Description = fromObject.Description;
            item.Item.Id = fromObject.Id;
            item.ItemId = fromObject.ItemId;
            item.Item.ItemType = fromObject.ItemType;
            item.Item.Price = fromObject.Price;
            item.Quantity = fromObject.Quantity;

            return item;
        }
    }
}
