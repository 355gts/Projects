using System;
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
        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool RequiresAction
        {
            get { return string.IsNullOrEmpty(PlanPath); }
        }

        public string PlanPath { get; set; }

        public bool MemberOfHallOfFame { get; set; }

        public string BeforeImage { get; set; }

        public string AfterImage { get; set; }

        public string Comment { get; set; }

        public DateTime? HallOfFameDate { get; set; }

        public bool HallOfFameEnabled { get; set; }
    }
}
