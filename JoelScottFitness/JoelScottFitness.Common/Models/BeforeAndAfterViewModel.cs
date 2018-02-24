using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class BeforeAndAfterViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long PurchasedItemId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public HttpPostedFileBase BeforeFile { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public HttpPostedFileBase AfterFile { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Comment { get; set; }
    }
}
