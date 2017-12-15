using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CustomerViewModel : CreateCustomerViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public long BillingAddressId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime CreatedDate { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime? ModifiedDate { get; set; }

        [DataMember(IsRequired = false)]
        public ICollection<PurchaseHistoryViewModel> PurchaseHistory { get; set; }
    }
}
