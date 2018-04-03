using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ItemViewModel : CreateItemViewModel
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool ItemDiscontinued { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public DateTime CreatedDate { get; set; }

        [DataMember(IsRequired = false)]
        public DateTime? ModifiedDate { get; set; }

    }
}
