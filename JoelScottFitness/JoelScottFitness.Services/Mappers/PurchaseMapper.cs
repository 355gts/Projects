using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PurchaseMapper : ITypeMapper<PurchaseViewModel, Purchase>, ITypeMapper<Purchase, PurchaseViewModel>
    {
        PurchasedItemMapper purchasedItemMapper = new PurchasedItemMapper();

        public Purchase Map(PurchaseViewModel fromObject, Purchase toObject = null)
        {
            var purchase = toObject ?? new Purchase();

            purchase.CustomerId = fromObject.CustomerId;
            purchase.DiscountCodeId = fromObject.DiscountCodeId;
            purchase.Id = fromObject.Id;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = fromObject.PurchaseDate;
            purchase.SalesReference = fromObject.SalesReference;
            purchase.TotalAmount = fromObject.TotalAmount;

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                var items = new List<PurchasedItem>();
                foreach (var item in fromObject.Items)
                {
                    items.Add(purchasedItemMapper.Map(item));
                }
                purchase.Items = items;
            }

            return purchase;
        }

        public PurchaseViewModel Map(Purchase fromObject, PurchaseViewModel toObject = null)
        {
            var purchase = toObject ?? new PurchaseViewModel();

            purchase.CustomerId = fromObject.CustomerId;
            purchase.DiscountCodeId = fromObject.DiscountCodeId;
            purchase.Id = fromObject.Id;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = fromObject.PurchaseDate;
            purchase.SalesReference = fromObject.SalesReference;
            purchase.TotalAmount = fromObject.TotalAmount;

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                var items = new List<PurchasedItemViewModel>();
                foreach (var item in fromObject.Items)
                {
                    items.Add(purchasedItemMapper.Map(item));
                }
                purchase.Items = items;
            }

            return purchase;
        }
    }
}
