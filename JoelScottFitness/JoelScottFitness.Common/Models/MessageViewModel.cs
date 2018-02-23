using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class MessageViewModel : CreateMessageViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int64.MaxValue)]
        public long Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Received Date")]
        public DateTime ReceivedDate { get; set; }

        public bool Responded { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Response { get; set; }
    }
}
