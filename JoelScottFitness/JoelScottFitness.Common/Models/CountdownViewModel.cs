using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CountdownViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public DateTime GoLiveDate { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool CountdownEnabled { get; set; }
    }
}
