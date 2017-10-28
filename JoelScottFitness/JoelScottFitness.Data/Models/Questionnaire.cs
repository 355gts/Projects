﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoelScottFitness.Data.Models
{
    public class Questionnaire : BaseRecord
    {
        [Required]
        [ForeignKey("Purchase")]
        public long PurchaseId { get; set; }

        public Purchase Purchase { get; set; }

        [Required]
        public string Age { get; set; }

        [Required]
        public string Weight { get; set; }

        [Required]
        public string Height { get; set; }

        [Required]
        public bool IsMemberOfGym { get; set; }

        public string CurrentGym { get; set; }

        [Required]
        public long WorkoutTypeId { get; set; }

        public string WorkoutDescription { get; set; }

        [Required]
        public long DietTypeId { get; set; }

        public string DietDetails { get; set; }

        public string TrainingGoals { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}
