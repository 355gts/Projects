using JoelScottFitness.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Item : BaseRecord
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public ItemCategory ItemCategory { get; set; }

        [Required]
        public bool ItemDiscontinued { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
