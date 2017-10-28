using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class QuestionnaireViewModel : CreateQuestionnaireViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long Id { get; set; }
    }
}
