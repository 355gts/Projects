using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class OrderItem : BaseRecord
    {
        [ForeignKey("Item")]
        [Required]
        public long ItemId { get; set; }

        public Item Item { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double Total { get; set; }

        [Required]
        public ItemCategory ItemCategory { get; set; }

        [Required]
        public long Quantity { get; set; }

        [Required]
        public bool ItemDiscounted { get; set; }

        [ForeignKey("Order")]
        [Required]
        public long OrderId { get; set; }

        public Order Order { get; set; }
    }
}
