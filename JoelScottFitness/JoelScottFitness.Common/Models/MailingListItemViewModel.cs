using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class MailingListItemViewModel : BaseViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
