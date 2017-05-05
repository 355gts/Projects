using JoelScottFitness.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class ItemViewModel : BaseViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        [Required]
        public bool ItemDiscontinued { get; set; }
    }
}
