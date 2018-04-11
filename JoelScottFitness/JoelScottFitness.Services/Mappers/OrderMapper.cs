using JoelScottFitness.Common.Extensions;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class OrderMapper : ITypeMapper<Order, OrderHistoryViewModel>
    {
        OrderItemMapper orderItemMapper = new OrderItemMapper();

        public OrderHistoryViewModel Map(Order fromObject, OrderHistoryViewModel toObject = null)
        {
            var order = toObject ?? new OrderHistoryViewModel();

            order.CustomerId = fromObject.CustomerId.Value;
            order.DiscountCodeId = fromObject.DiscountCodeId;
            order.Id = fromObject.Id;
            order.PayPalReference = fromObject.PayPalReference;
            order.PurchaseDate = fromObject.PurchaseDate;
            order.PurchaseDateDisplayString = fromObject.PurchaseDate.DateTimeDisplayStringLong();
            order.TransactionId = fromObject.TransactionId;
            order.TotalAmount = fromObject.TotalAmount;
            order.QuestionnaireId = fromObject.QuestionnareId;
            order.RequiresAction = fromObject.RequiresAction;

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                var items = new List<OrderItemViewModel>();
                foreach (var item in fromObject.Items)
                {
                    items.Add(orderItemMapper.Map(item));
                }
                order.Items = items;
            }

            return order;
        }
    }
}
