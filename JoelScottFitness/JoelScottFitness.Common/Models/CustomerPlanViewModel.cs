using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CustomerPlanViewModel : CreateCustomerPlanViewModel
    {
        [DataMember(IsRequired = true)]
        public long Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public string ImagePath { get; set; }

        [DataMember(IsRequired = true)]
        public string PlanPath { get; set; }

        [DataMember(IsRequired = true)]
        public string SheetsUri { get; set; }

        [DataMember(IsRequired = true)]
        public bool QuestionnaireComplete { get; set; }
    }
}
