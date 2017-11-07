using JoelScottFitness.Common.Enumerations;
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

        [Required]
        [DataMember(IsRequired = true)]
        public long Quantity { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double Price { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public ItemType ItemType { get; set; }
    }
}
