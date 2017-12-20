using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateCustomerViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Confirm Email")]
        [Compare("EmailAddress", ErrorMessage = "The emails entered do not match.")]
        public string ConfirmEmailAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Firstname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Surname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public AddressViewModel BillingAddress { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool JoinMailingList { get; set; }
    }
}
