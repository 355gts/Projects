using JoelScottFitness.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Item : BaseRecord
    {
        [Required]
        public double Price { get; set; }

        [Required]
        public ItemType ItemType { get; set; }
    }
}
