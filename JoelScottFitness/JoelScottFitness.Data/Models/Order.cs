﻿using JoelScottFitness.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class Order : BaseRecord
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
        public Guid? CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public ICollection<OrderItem> Items { get; set; }

        [ForeignKey("Questionnaire")]
        public long? QuestionnareId { get; set; }

        public Questionnaire Questionnaire { get; set; }

        [Required]
        public bool RequiresAction { get; set; }
    }
}
