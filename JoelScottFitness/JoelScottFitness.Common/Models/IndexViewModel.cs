using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class IndexViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public IEnumerable<BlogViewModel> Blogs { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public IEnumerable<MediaViewModel> Videos { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public SectionImageViewModel SectionImages { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public KaleidoscopeViewModel KaleidoscopeImages { get; set; }
        
        [DataMember(IsRequired = false)]
        public HallOfFameViewModel LatestHallOfFamer { get; set; }

    }
}
