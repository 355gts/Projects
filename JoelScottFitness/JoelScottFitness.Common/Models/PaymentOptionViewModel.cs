using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PaymentOptionViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public Guid CustomerId { get; set; }
    }
}
