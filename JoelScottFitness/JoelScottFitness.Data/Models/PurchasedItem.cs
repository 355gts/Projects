using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class PurchasedItem : BaseRecord
    {
        [ForeignKey("Item")]
        [Required]
        public long ItemId { get; set; }

        public Item Item { get; set; }

        [Required]
        public long Quantity { get; set; }

        [ForeignKey("Purchase")]
        [Required]
        public long PurchaseId { get; set; }

        public Purchase Purchase { get; set; }
    }
}
