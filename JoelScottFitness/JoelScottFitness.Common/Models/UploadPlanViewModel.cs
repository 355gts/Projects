using System;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class UploadPlanViewModel
    {
        [DataMember(IsRequired = true)]
        public long PlanId { get; set; }

        [DataMember(IsRequired = true)]
        public long OrderId { get; set; }

        [DataMember(IsRequired = true)]
        public Guid CustomerId { get; set; }

        [DataMember(IsRequired = true)]
        public string SheetsUri { get; set; }
    }
}
