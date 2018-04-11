using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class OrderItemViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long ItemId { get; set; }

        public ItemViewModel Item { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long Quantity { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double Price { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double ItemTotal { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public ItemCategory ItemCategory { get; set; }

        [DataMember(IsRequired = false)]
        public string TransactionId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool ItemDiscounted { get; set; }
    }
}
