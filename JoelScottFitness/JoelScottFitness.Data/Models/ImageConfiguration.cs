using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class ImageConfiguration : BaseRecord
    {
        [Required]
        public long SplashImageId { get; set; }

        [Required]
        public long SectionImage1Id { get; set; }

        [Required]
        public long SectionImage2Id { get; set; }

        [Required]
        public long SectionImage3Id { get; set; }
        
        [Required]
        public bool Randomize { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}
