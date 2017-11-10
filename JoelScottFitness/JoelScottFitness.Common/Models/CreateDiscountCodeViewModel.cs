using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateDiscountCodeViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Code { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public int PercentDiscount { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ValidFrom { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime ValidTo { get; set; }
    }
}
