using JoelScottFitness.Common.Enumerations;
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
        public BasketViewModel Basket { get; set; }

        [Required]
        public string PayPalReference { get; set; }

        [Required]
        public string TransactionId { get; set; }

        [Required]
        public PurchaseStatus PurchaseStatus { get; set; }
    }
}
