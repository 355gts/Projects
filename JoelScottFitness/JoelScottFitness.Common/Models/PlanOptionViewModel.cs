using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class PlanOptionViewModel : BaseViewModel
    {
        [Required]
        public double Price { get; set; }

        [Required]

        public ItemType ItemType { get; set; }

        [Required]
        public long Duration { get; set; }
        
    }
}
