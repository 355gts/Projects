﻿using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace JoelScottFitness.Services.Mappers
{
    sealed class PurchaseSummaryMapper : ITypeMapper<Purchase, PurchaseSummaryViewModel>
    {
        PurchasedItemMapper purchasedItemMapper = new PurchasedItemMapper();

        public PurchaseSummaryViewModel Map(Purchase fromObject, PurchaseSummaryViewModel toObject = null)
        {
            var purchase = toObject ?? new PurchaseSummaryViewModel();

            purchase.CustomerId = fromObject.CustomerId;
            purchase.CustomerName = $"{fromObject.Customer?.Firstname} {fromObject.Customer?.Surname}";
            purchase.DiscountCodeId = fromObject.DiscountCodeId;
            purchase.DiscountCode = fromObject.DiscountCode?.Code;
            purchase.Id = fromObject.Id;
            purchase.PayPalReference = fromObject.PayPalReference;
            purchase.PurchaseDate = fromObject.PurchaseDate;
            purchase.TransactionId = fromObject.TransactionId;
            purchase.TotalAmount = fromObject.TotalAmount;
            purchase.QuestionnaireComplete = fromObject.Questionnaire != null;

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                var items = new List<PurchasedHistoryItemViewModel>();
                foreach (var item in fromObject.Items)
                {
                    items.Add(purchasedItemMapper.Map(item));
                }
                purchase.Items = items;
            }

            if (fromObject.Items != null && fromObject.Items.Any())
            {
                purchase.RequiresAction = purchase.Items.Any(p => p.RequiresAction);
            }

            return purchase;
        }
    }
}
