using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PlanOptionViewModel : ItemViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long PlanId { get; set; }

        public PlanViewModel Plan { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long Duration { get; set; }
    }
}
