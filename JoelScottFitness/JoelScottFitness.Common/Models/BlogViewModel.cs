using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class BlogViewModel : BaseViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string SubHeader { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ImagePath { get; set; }
        
        public bool Active { get; set; }
    }
}
