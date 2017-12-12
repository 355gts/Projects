using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PurchaseHistoryViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PurchaseDateDisplayString { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double TotalAmount { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PayPalReference { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string TransactionId { get; set; }

        [DataMember(IsRequired = false)]
        public long? DiscountCodeId { get; set; }

        [DataMember(IsRequired = false)]
        public DiscountCodeViewModel DiscountCode { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public long CustomerId { get; set; }

        [DataMember(IsRequired = true)]
        public CustomerViewModel Customer { get; set; }

        [DataMember(IsRequired = false)]
        public long? QuestionnaireId { get; set; }

        [DataMember(IsRequired = false)]
        public QuestionnaireViewModel Questionnaire { get; set; }

        [DataMember(IsRequired = true)]
        public ICollection<PurchasedHistoryItemViewModel> Items { get; set; }
    }
}
