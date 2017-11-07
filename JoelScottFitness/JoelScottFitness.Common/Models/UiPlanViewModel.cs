using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    public class UiPlanViewModel : PlanViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long SelectedOptionId { get; set; }
    }
}
