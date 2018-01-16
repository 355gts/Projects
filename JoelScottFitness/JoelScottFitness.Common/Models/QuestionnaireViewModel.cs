using JoelScottFitness.Common.OptionLists;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class QuestionnaireViewModel : BaseViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public long PurchaseId { get; set; }
        
        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue, ErrorMessage = "Age must be a number")]
        public string Age { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field Weight must be a number")]
        [Display(Name = "Weight (lbs)")]
        public string Weight { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field Height must be a number")]
        [Display(Name = "Height (inches)")]
        public string Height { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Member Of Gym?")]
        public bool IsMemberOfGym { get; set; }

        public List<KeyValuePair<bool, string>> BooleanTypes { get { return Options.TrueFalseTypes; } }

        [DataMember(IsRequired = false)]
        [Display(Name = "Current Gym")]
        public string CurrentGym { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Workout Type")]
        public long WorkoutTypeId { get; set; }

        public List<KeyValuePair<int, string>> WorkoutTypes { get { return Options.WorkoutTypes; } }

        [DataMember(IsRequired = false)]
        [Display(Name = "Workout Description")]
        public string WorkoutDescription { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Diet Type")]
        public long DietTypeId { get; set; }

        public List<KeyValuePair<int, string>> DietTypes { get { return Options.DietTypes; } }

        [DataMember(IsRequired = false)]
        [Display(Name = "Diet Details")]
        public string DietDetails { get; set; }

        [DataMember(IsRequired = false)]
        [Display(Name = "Training Goals")]
        public string TrainingGoals { get; set; }
    }
}
