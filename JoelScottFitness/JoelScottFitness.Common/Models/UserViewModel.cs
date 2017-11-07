using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class UserViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string EmailAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }
    }
}
