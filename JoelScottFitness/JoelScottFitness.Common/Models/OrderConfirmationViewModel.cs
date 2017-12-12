using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class OrderConfirmationViewModel
    {
        [DataMember(IsRequired = true)]
        public string OrderReference { get; set; }
    }
}
