using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class MediaViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string VideoId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Description { get; set; }
    }
}
