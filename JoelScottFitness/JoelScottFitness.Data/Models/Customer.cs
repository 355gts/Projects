using JoelScottFitness.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class Customer : BaseRecord
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        
        [Required]
        public Address BillingAddress { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }

        public AuthUser User { get; set; }

        public ICollection<Purchase> PurchaseHistory { get; set; }
    }
}
