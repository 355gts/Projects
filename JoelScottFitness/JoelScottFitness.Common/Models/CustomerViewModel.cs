using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoelScottFitness.Common.Models
{
    public class CustomerViewModel : BaseViewModel
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public AddressViewModel BillingAddress { get; set; }
        
        public string UserId { get; set; }
        
        public ICollection<PurchaseViewModel> PurchaseHistory { get; set; }
    }
}
