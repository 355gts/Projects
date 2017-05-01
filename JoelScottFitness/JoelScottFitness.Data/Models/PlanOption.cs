using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class PlanOption : Item
    {
        [Required]
        public long Duration { get; set; }

        [ForeignKey("Plan")]
        [Required]
        public long PlanId { get; set; }

        public Plan Plan { get; set; }
    }
}
