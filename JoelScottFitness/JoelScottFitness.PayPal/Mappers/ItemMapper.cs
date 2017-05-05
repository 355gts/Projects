using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using PayPal.Api;

namespace JoelScottFitness.PayPal.Mappers
{
    sealed class ItemMapper : ITypeMapper<PurchasedItemViewModel, Item>
    {
        public Item Map(PurchasedItemViewModel fromObject, Item toObject = null)
        {
            var item = toObject ?? new Item();

            item.currency = "GBP";
            item.description = fromObject.Description;
            item.name = fromObject.Description;
            item.price = fromObject.Price.ToString();
            item.quantity = fromObject.Quantity.ToString();
            item.sku = fromObject.Id.ToString();

            return item;
        }
    }
}
