using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class CreatePurchaseMapper : ITypeMapper<ConfirmPurchaseViewModel, Purchase>
    {
        CreatePurchaseItemMapper createPurchasedItemMapper = new CreatePurchaseItemMapper();

        public Purchase Map(ConfirmPurchaseViewModel fromObject, Purchase toObject = null)
        {
            var purchase = toObject ?? new Purchase();

            purchase.CustomerId = fromObject.CustomerDetails.Id;
            purchase.DiscountCodeId = fromObject.DiscountCodeId;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = DateTime.UtcNow;
            purchase.Status = PurchaseStatus.Complete;
            purchase.TotalAmount = fromObject.BasketItems.Sum(i => (i.Price * i.Quantity));
            purchase.TransactionId = fromObject.TransactionId;

            if (fromObject.BasketItems != null && fromObject.BasketItems.Any())
            {
                var items = new List<PurchasedItem>();
                foreach (var item in fromObject.BasketItems)
                {
                    items.Add(createPurchasedItemMapper.Map(item));
                }
                purchase.Items = items;
            }

            return purchase;
        }
    }
}
