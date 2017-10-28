using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ConfirmPurchaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public CustomerViewModel CustomerDetails { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public IEnumerable<PlanOptionViewModel> BasketItems { get; set; }

        [DataMember(IsRequired = false)]
        public long? DiscountCodeId { get; set; }

        [Required]
        public string PayPalReference { get; set; }

        [Required]
        public string TransactionId { get; set; }
    }
}
