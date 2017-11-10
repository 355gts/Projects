using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class DiscountCodeViewModel : CreateDiscountCodeViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        public bool Active
        {
            get
            {
                return ValidFrom <= DateTime.UtcNow 
                    && DateTime.UtcNow <= ValidTo;
            }
        }
    }
}
