using JoelScottFitness.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class PlanViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BannerHeader { get; set; }

        [Required]
        public string ImagePathMedium { get; set; }

        [Required]
        public string ImagePathLarge { get; set; }

        [Required]
        public Gender TargetGender { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        public bool Active { get; set; }

        public ICollection<PlanOptionViewModel> Options { get; set; }
    }
}
