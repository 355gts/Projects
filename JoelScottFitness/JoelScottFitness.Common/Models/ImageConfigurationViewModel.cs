using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ImageConfigurationViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long SplashImageId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long SectionImage1Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long SectionImage2Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long SectionImage3Id { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public bool Randomize { get; set; }
    }
}
