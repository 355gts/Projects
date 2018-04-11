using System;
using System.Runtime.Serialization;
using System.Web;

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
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}
