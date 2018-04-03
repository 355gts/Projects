using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateCustomerPlanViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long ItemId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public Guid CustomerId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool RequiresAction { get; set; }
    }
}
