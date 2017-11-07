using JoelScottFitness.Common.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class CreatePlanViewModel
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Description { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public string BannerHeader { get; set; }
        
        [DataMember(IsRequired = false)]
        public string ImagePathMedium { get; set; }
        
        [DataMember(IsRequired = false)]
        public string ImagePathLarge { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        public Gender TargetGender { get; set; }

        public List<KeyValuePair<string, int>> GenderTypes
        {
            get
            {
                return new List<KeyValuePair<string, int>>()
                {
                    new KeyValuePair<string, int>(Gender.Male.ToString(), (int)Gender.Male),
                    new KeyValuePair<string, int>(Gender.Female.ToString(), (int)Gender.Female),
                };
            }
        }

        public virtual ICollection<CreatePlanOptionViewModel> Options { get; set; }
    }
}
