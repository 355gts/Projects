using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PurchasedItemMapper : ITypeMapper<PurchasedItem, PurchasedItemViewModel>, ITypeMapper<PurchasedItemViewModel, PurchasedItem>
    {
        public PurchasedItemViewModel Map(PurchasedItem fromObject, PurchasedItemViewModel toObject = null)
        {
            var item = toObject ?? new PurchasedItemViewModel();
            
            item.Description = fromObject.Description;
            item.Id = fromObject.Id;
            item.ItemId = fromObject.ItemId;
            item.ItemType = fromObject.ItemType;
            item.Price = fromObject.Price;
            item.Quantity = fromObject.Quantity;

            return item;
        }

        public PurchasedItem Map(PurchasedItemViewModel fromObject, PurchasedItem toObject = null)
        {
            var item = toObject ?? new PurchasedItem();

            item.Description = fromObject.Description;
            item.Id = fromObject.Id;
            item.ItemId = fromObject.ItemId;
            item.ItemType = fromObject.ItemType;
            item.Price = fromObject.Price;
            item.Quantity = fromObject.Quantity;

            return item;
        }
    }
}
