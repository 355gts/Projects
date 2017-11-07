using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateItemViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public double Price { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public ItemType ItemType { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public bool ItemDiscontinued { get; set; }
    }
}
