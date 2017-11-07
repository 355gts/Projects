using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Blog : BaseRecord
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string SubHeader { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public bool Active { get; set; }

        public ICollection<BlogImage> BlogImages {get; set;}

    }
}
