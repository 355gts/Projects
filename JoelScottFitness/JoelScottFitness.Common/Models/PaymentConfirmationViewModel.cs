using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PaymentConfirmationViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string TransactionId { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string PlanQuestionnaireUrl { get; set; }
    }
}
