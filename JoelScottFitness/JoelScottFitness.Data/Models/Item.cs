using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

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
    }
}
