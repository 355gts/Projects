using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class ItemQuantityViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long Quantity { get; set; }
        
        public double Price { get; set; }
    }
}
