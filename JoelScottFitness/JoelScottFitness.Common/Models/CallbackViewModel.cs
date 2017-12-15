using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CallbackViewModel
    {
        [DataMember(IsRequired = true)]
        public string CallbackUrl { get; set; }
    }
}
