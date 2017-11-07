using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BlogViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

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

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime CreatedDate { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime? ModifiedDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool Active { get; set; }

        public IEnumerable<BlogImageViewModel> BlogImages { get; set; }
    }
}
