using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateBlogViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string SubHeader { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        public string Content { get; set; }
        
        [DataMember(IsRequired = false)]
        public string ImagePath { get; set; }

        public IEnumerable<CreateBlogImageViewModel> BlogImages { get; set; }
    }
}
