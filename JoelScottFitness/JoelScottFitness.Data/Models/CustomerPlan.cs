using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class CustomerPlan : BaseRecord
    {
        [ForeignKey("Order")]
        [Required]
        public long OrderId { get; set; }

        public Order Order { get; set; }

        [ForeignKey("Item")]
        [Required]
        public long ItemId { get; set; }

        public Item Item { get; set; }

        [ForeignKey("Customer")]
        [Required]
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool RequiresAction
        {
            get
            {
                return string.IsNullOrEmpty(SheetsUri);
            }
        }

        public bool MemberOfHallOfFame { get; set; }

        public string BeforeImage { get; set; }

        public string AfterImage { get; set; }

        public string Comment { get; set; }

        public DateTime? HallOfFameDate { get; set; }

        public bool HallOfFameEnabled { get; set; }

        [Required]
        public bool QuestionnaireComplete { get; set; }

        public string SheetsUri { get; set; }
    }
}
