using JoelScottFitness.Common.OptionLists;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreateQuestionnaireViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long PurchaseId { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue)]
        public string Age { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue)]
        public string Weight { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue)]
        public string Height { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public bool IsMemberOfGym { get; set; }

        [DataMember(IsRequired = false)]
        public string CurrentGym { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long WorkoutTypeId { get; set; }

        public List<KeyValuePair<int, string>> WorkoutTypes { get { return Options.WorkoutTypes; } }

        [DataMember(IsRequired = false)]
        public string WorkoutDescription { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public long DietTypeId { get; set; }

        public List<KeyValuePair<int, string>> DietTypes { get { return Options.DietTypes; } }

        [DataMember(IsRequired = false)]
        public string DietDetails { get; set; }

        [DataMember(IsRequired = false)]
        public string TrainingGoals { get; set; }
    }
}
