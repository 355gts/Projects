using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class MailingListItemViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool Active { get; set; }
    }
}
