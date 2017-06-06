using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    [Table("MailingList")]
    public class MailingListItem : BaseRecord
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
