using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateOrderMapper : ITypeMapper<ConfirmOrderViewModel, Order>
    {
        AddOrderItemMapper createOrderItemMapper = new AddOrderItemMapper();

        public Order Map(ConfirmOrderViewModel fromObject, Order toObject = null)
        {
            var order = toObject ?? new Order();

            order.CustomerId = fromObject.CustomerDetails.Id;
            order.DiscountCodeId = fromObject.Basket?.DiscountCode?.Id;
            order.PayPalReference = fromObject.PayPalReference;
            order.PurchaseDate = DateTime.UtcNow;
            order.Status = OrderStatus.Complete;
            order.TotalAmount = fromObject.Basket.Total;
            order.TransactionId = fromObject.TransactionId;

            if (fromObject.Basket != null && fromObject.Basket.Items != null && fromObject.Basket.Items.Any())
            {
                var items = new List<OrderItem>();
                foreach (var item in fromObject.Basket.Items.Select(s => s.Value).ToList())
                {
                    items.Add(createOrderItemMapper.Map(item));
                }
                order.Items = items;
            }

            return order;
        }
    }
}
