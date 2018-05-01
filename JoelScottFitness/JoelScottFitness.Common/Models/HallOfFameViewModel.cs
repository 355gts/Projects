using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class HallOfFameViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long PlanId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string BeforeImagePath { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string AfterImagePath { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Comment { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PlanName { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PlanDescription { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool Enabled { get; set; }
    }
}
