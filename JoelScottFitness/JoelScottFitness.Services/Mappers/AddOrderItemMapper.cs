using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class AddOrderItemMapper : ITypeMapper<BasketItemViewModel, OrderItem>
    {
        public OrderItem Map(BasketItemViewModel fromObject, OrderItem toObject = null)
        {
            var item = toObject ?? new OrderItem();

            item.ItemId = fromObject.Id;
            item.Price = fromObject.ItemDiscounted ? fromObject.DiscountedPrice : fromObject.Price;
            item.Quantity = fromObject.Quantity;
            item.ItemCategory = fromObject.ItemCategory;
            item.ItemDiscounted = fromObject.ItemDiscounted;
            item.Total = fromObject.ItemTotal;

            return item;
        }
    }
}
