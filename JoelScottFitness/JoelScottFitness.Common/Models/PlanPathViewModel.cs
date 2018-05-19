using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class PlanPathViewModel
    {
        [DataMember(IsRequired = true)]
        public string PlanPath { get; set; }

        [DataMember(IsRequired = true)]
        public string SheetsUri { get; set; }
    }
}
