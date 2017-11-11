using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ImageViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string ImagePath { get; set; }
    }
}
