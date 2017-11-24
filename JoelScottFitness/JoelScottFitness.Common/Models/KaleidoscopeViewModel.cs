using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class KaleidoscopeViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Image1 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Image2 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Image3 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Image4 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Image5 { get; set; }
    }
}
