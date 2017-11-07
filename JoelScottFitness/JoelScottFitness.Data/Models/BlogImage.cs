using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class BlogImage : BaseRecord
    {
        [ForeignKey("Blog")]
        [Required]
        public long BlogId { get; set; }

        public Blog Blog { get; set; }

        [Required]
        public string ImagePath { get; set; }
        
        public string CaptionTitle { get; set; }
        
        public string Caption { get; set; }

        public BlogCaptionTextColour CaptionColour { get; set; }
    }
}
