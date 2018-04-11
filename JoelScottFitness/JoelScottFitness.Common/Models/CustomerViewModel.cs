using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CustomerViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Firstname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Surname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long BillingAddressId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public AddressViewModel BillingAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Join Mailing List")]
        public bool JoinMailingList { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public DateTime CreatedDate { get; set; }

        public long? UserId { get; set; }

        public UserViewModel User { get; set; }

        [DataMember(IsRequired = false)]
        public ICollection<OrderHistoryViewModel> PurchaseHistory { get; set; }
    }
}
