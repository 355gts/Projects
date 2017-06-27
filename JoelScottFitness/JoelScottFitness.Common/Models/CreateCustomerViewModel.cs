using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class CreateCustomerViewModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ConfirmEmailAddress { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public AddressViewModel BillingAddress { get; set; }

        public long? UserId { get; set; }

        public UserViewModel User { get; set; }

        public bool RegisterAccount { get; set; }

        public bool JoinMailingList { get; set; }


    }
}
