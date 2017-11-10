using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class DiscountCode : BaseRecord
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public int PercentDiscount { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }
    }
}

