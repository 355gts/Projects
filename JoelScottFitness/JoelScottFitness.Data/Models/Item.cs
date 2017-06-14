using JoelScottFitness.Common.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class Item : BaseRecord
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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool ItemDiscontinued { get { return ActiveTo == null; } }
    }
}
