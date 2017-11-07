using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateCustomerViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Firstname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Surname { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataMember(IsRequired = true)]
        [DataType(DataType.EmailAddress)]
        public string ConfirmEmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public AddressViewModel BillingAddress { get; set; }

        public long? UserId { get; set; }

        public UserViewModel User { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool RegisterAccount { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool JoinMailingList { get; set; }
    }
}
