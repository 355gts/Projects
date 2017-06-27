using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class UserViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
