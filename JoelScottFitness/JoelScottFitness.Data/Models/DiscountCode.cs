using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool Active
        {
            get
            {
                var currentDate = DateTime.UtcNow;
                return ValidFrom <= currentDate && currentDate <= ValidTo;
            }
        }
    }
}

