using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateMessageViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Subject { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Message { get; set; }

        [DataMember(IsRequired = false)]
        [Display(Name = "Join Mailing List")]
        public bool JoinMailingList { get; set; }
    }
}
