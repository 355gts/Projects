﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BlogImageViewModel : CreateBlogImageViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long BlogId { get; set; }
    }
}
