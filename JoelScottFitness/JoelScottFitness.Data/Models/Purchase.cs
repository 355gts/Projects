using JoelScottFitness.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class Purchase : BaseRecord
    {
        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public double TotalAmount { get; set; }

        [Required]
        public string PayPalReference { get; set; }

        [Required]
        public string TransactionId { get; set; }
        
        [ForeignKey("DiscountCode")]
        public long? DiscountCodeId { get; set; }

        public DiscountCode DiscountCode { get; set; }
        
        [ForeignKey("Customer")]
        [Required]
        public long CustomerId { get; set; }
        
        public Customer Customer { get; set; }

        [Required]
        public PurchaseStatus Status { get; set; }

        public ICollection<PurchasedItem> Items { get; set; }

        [ForeignKey("Questionnaire")]
        public long? QuestionnareId { get; set; }

        public Questionnaire Questionnaire { get; set; }
    }
}
