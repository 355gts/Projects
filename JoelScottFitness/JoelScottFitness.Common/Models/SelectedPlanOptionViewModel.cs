using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class SelectedPlanOptionViewModel : PlanOptionViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Quantity { get; set; }
    }
}
