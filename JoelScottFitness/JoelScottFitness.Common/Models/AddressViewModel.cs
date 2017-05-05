using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class AddressViewModel : BaseViewModel
    {
        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string CountryCode { get; set; }
    }
}
