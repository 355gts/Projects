using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreateOrderMapper : ITypeMapper<ConfirmPurchaseViewModel, Order>
    {
        AddOrderItemMapper createPurchasedItemMapper = new AddOrderItemMapper();

        public Order Map(ConfirmPurchaseViewModel fromObject, Order toObject = null)
        {
            var purchase = toObject ?? new Order();

            purchase.CustomerId = fromObject.CustomerDetails.Id;
            purchase.DiscountCodeId = fromObject.Basket?.DiscountCode?.Id;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = DateTime.UtcNow;
            purchase.Status = PurchaseStatus.Complete;
            purchase.TotalAmount = fromObject.Basket.Total;
            purchase.TransactionId = fromObject.TransactionId;

            if (fromObject.Basket != null && fromObject.Basket.Items != null && fromObject.Basket.Items.Any())
            {
                var items = new List<OrderItem>();
                foreach (var item in fromObject.Basket.Items.Select(s => s.Value).ToList())
                {
                    items.Add(createPurchasedItemMapper.Map(item));
                }
                purchase.Items = items;
            }

            return purchase;
        }
    }
}
