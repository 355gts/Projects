using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class DiscountCodeViewModel : BaseViewModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public int PercentDiscount { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }
        
        public bool Active { get; set; }
    }
}
