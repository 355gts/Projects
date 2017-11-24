﻿using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PurchasedHistoryItemViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long ItemId { get; set; }

        public ItemViewModel Item { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long PlanOptionId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long Quantity { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double Price { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public ItemType ItemType { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool RequiresAction { get; set; }

        [DataMember(IsRequired = false)]
        public string PlanPath { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool QuestionnaireComplete { get; set; }

        [DataMember(IsRequired = false)]
        public string TransactionId { get; set; }
    }
}
