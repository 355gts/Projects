using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class PurchasedItemViewModel : BaseViewModel
    {
        [Required]
        public long ItemId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public ItemType ItemType { get; set; }
    }
}
