using System;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Message : BaseRecord
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }

        [Required]
        public string Content { get; set; }

        public bool Responded { get; set; }

        public string Response { get; set; }
    }
}
