using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ResetPasswordCallbackViewModel
    {
        [DataMember(IsRequired = true)]
        public string CallbackUrl { get; set; }
    }
}
