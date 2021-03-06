﻿using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Questionnaire : BaseRecord
    {
        [Required]
        public int Age { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public int Height { get; set; }

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
    }
}
