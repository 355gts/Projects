using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class PlanOptionViewModel : ItemViewModel
    {
        [Required]
        public long PlanId { get; set; }

        public PlanViewModel Plan { get; set; }

        [Required]
        public long Duration { get; set; }
    }
}
