using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using PayPal.Api;

namespace JoelScottFitness.PayPal.Mappers
{
    sealed class ItemMapper : ITypeMapper<BasketItemViewModel, Item>
    {
        public Item Map(BasketItemViewModel fromObject, Item toObject = null)
        {
            var item = toObject ?? new Item();

            item.currency = "GBP";
            item.description = fromObject.Description;
            item.name = fromObject.Name;
            item.price = fromObject.ItemDiscounted ? fromObject.DiscountedPrice.ToString() : fromObject.Price.ToString();
            item.quantity = fromObject.Quantity.ToString();
            item.sku = fromObject.Id.ToString();

            return item;
        }
    }
}
