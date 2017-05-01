using System;
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
        public string CreatedDate { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        public DateTime ActiveTo { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string ImagePath { get; set; }
    }
}
