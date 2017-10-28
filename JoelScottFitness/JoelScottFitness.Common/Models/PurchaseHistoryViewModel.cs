using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class PurchaseHistoryViewModel : BaseViewModel
    {
        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public string PayPalReference { get; set; }

        [Required]
        public string TransactionId { get; set; }
        
        public long? DiscountCodeId { get; set; }

        public DiscountCodeViewModel DiscountCode { get; set; }
        
        [Required]
        public long CustomerId { get; set; }
        
        public ICollection<PurchasedHistoryItemViewModel> Items { get; set; }
    }
}
