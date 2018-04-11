using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;

namespace JoelScottFitness.Services.Mappers
{
    sealed class OrderItemMapper : ITypeMapper<OrderItem, OrderItemViewModel>, ITypeMapper<OrderItemViewModel, OrderItem>
    {
        ItemMapper itemMapper = new ItemMapper();

        public OrderItemViewModel Map(OrderItem fromObject, OrderItemViewModel toObject = null)
        {
            var orderItem = toObject ?? new OrderItemViewModel();

            orderItem.Item = itemMapper.Map(fromObject.Item);

            orderItem.Id = fromObject.Id;
            orderItem.ItemId = fromObject.ItemId;
            orderItem.ItemCategory = fromObject.ItemCategory;
            orderItem.ItemDiscounted = fromObject.ItemDiscounted;
            orderItem.OrderId = fromObject.OrderId;
            orderItem.Price = fromObject.Price;
            orderItem.Quantity = fromObject.Quantity;
            orderItem.ItemTotal = fromObject.Total;

            return orderItem;
        }

        public OrderItem Map(OrderItemViewModel fromObject, OrderItem toObject = null)
        {
            var orderItem = toObject ?? new OrderItem();

            orderItem.Id = fromObject.Id;
            orderItem.ItemId = fromObject.ItemId;
            orderItem.ItemDiscounted = fromObject.ItemDiscounted;
            orderItem.OrderId = fromObject.OrderId;
            orderItem.ItemCategory = fromObject.ItemCategory;
            orderItem.Price = fromObject.Price;
            orderItem.Quantity = fromObject.Quantity;
            orderItem.Total = fromObject.ItemTotal;

            return orderItem;
        }
    }
}
