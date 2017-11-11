using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class SectionImageViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string SplashImage { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string SectionImage1 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string SectionImage2 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string SectionImage3 { get; set; }
    }
}
