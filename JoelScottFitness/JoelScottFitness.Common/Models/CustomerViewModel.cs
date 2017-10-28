using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class CustomerViewModel : CreateCustomerViewModel
    {
        [Required]
        public long Id { get; set; }
        
        [Required]
        public long BillingAddressId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }

        public ICollection<PurchaseHistoryViewModel> PurchaseHistory { get; set; }
    }
}
