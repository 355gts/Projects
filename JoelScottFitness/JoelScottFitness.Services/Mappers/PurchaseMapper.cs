using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PurchaseMapper : ITypeMapper<Purchase, PurchaseHistoryViewModel>
    {
        PurchasedItemMapper purchasedItemMapper = new PurchasedItemMapper();

        public PurchaseHistoryViewModel Map(Purchase fromObject, PurchaseHistoryViewModel toObject = null)
        {
            var purchase = toObject ?? new PurchaseHistoryViewModel();

            purchase.CustomerId = fromObject.CustomerId;
            purchase.DiscountCodeId = fromObject.DiscountCodeId;
            purchase.Id = fromObject.Id;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = fromObject.PurchaseDate;
            purchase.TransactionId = fromObject.TransactionId;
            purchase.TotalAmount = fromObject.TotalAmount;
            purchase.QuestionnaireId = fromObject.QuestionnareId;

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                var items = new List<PurchasedHistoryItemViewModel>();
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
