using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class OrderSummaryMapper : ITypeMapper<Order, OrderSummaryViewModel>
    {
        OrderItemMapper orderItemMapper = new OrderItemMapper();

        public OrderSummaryViewModel Map(Order fromObject, OrderSummaryViewModel toObject = null)
        {
            var order = toObject ?? new OrderSummaryViewModel();

            order.CustomerId = fromObject.CustomerId.Value;
            order.CustomerName = $"{fromObject.Customer?.Firstname} {fromObject.Customer?.Surname}";
            order.DiscountCodeId = fromObject.DiscountCodeId;
            order.DiscountCode = fromObject.DiscountCode?.Code;
            order.Id = fromObject.Id;
            order.PayPalReference = fromObject.PayPalReference;
            order.PurchaseDate = fromObject.PurchaseDate;
            order.TransactionId = fromObject.TransactionId;
            order.TotalAmount = fromObject.TotalAmount;
            order.QuestionnaireComplete = fromObject.Questionnaire != null;
            order.RequiresAction = fromObject.Customer?.Plans != null && fromObject.Customer.Plans
                                                                                            .Where(p => p.OrderId == fromObject.Id)
                                                                                            .Any(p => string.IsNullOrEmpty(p.PlanPath));


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
