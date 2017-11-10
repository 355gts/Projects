using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PurchaseSummaryViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string TransactionId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PayPalReference { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long CustomerId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string CustomerName { get; set; }

        [DataMember(IsRequired = false)]
        public long? DiscountCodeId { get; set; }

        [DataMember(IsRequired = false)]
        public string DiscountCode { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double TotalAmount { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool QuestionnaireComplete { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public bool RequiresAction { get; set; }

        public ICollection<PurchasedHistoryItemViewModel> Items { get; set; }
    }
}
