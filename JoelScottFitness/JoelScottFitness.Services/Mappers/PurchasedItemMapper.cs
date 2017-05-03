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

            item.Id = fromObject.Id;
            item.ItemType = fromObject.ItemType;
            item.OriginalItemId = fromObject.OriginalItemId;
            item.Price = fromObject.Price;

            return item;
        }

        public PurchasedItem Map(PurchasedItemViewModel fromObject, PurchasedItem toObject = null)
        {
            var item = toObject ?? new PurchasedItem();

            item.Id = fromObject.Id;
            item.ItemType = fromObject.ItemType;
            item.OriginalItemId = fromObject.OriginalItemId;
            item.Price = fromObject.Price;

            return item;
        }
    }
}
