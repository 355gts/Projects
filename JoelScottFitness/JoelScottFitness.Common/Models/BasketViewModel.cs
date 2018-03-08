using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BasketViewModel
    {
        [DataMember(IsRequired = false)]
        public DiscountCodeViewModel DiscountCode { get; set; }

        [DataMember(IsRequired = false)]
        public long? DiscountCodeId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public IEnumerable<SelectedPlanOptionViewModel> SelectedOptions { get; set; }
    }
}
