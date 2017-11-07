using JoelScottFitness.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateBlogImageViewModel
    {        
        [Required]
        [DataMember(IsRequired = true)]
        public string ImagePath { get; set; }

        public string CaptionTitle { get; set; }

        public string Caption { get; set; }

        public BlogCaptionTextColour CaptionColour { get; set; }
    }
}
