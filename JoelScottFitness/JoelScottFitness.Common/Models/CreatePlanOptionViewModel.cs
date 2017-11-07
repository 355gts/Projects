using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreatePlanOptionViewModel : CreateItemViewModel
    {        
        [Required]
        [DataMember(IsRequired = true)]
        public long Duration { get; set; }
    }
}
