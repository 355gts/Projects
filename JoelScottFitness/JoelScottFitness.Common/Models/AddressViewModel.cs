using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class AddressViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [DataMember(IsRequired = false)]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [DataMember(IsRequired = false)]
        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string City { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Region { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Country { get; set; }

        [DataMember(IsRequired = false)]
        public string CountryCode { get; set; }
    }
}
