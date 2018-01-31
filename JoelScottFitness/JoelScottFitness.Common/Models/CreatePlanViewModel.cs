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

        [Required]
        [DataMember(IsRequired = true)]
        public BannerColour BannerColour { get; set; }

        public List<KeyValuePair<string, int>> BannerColours
        {
            get
            {
                return new List<KeyValuePair<string, int>>()
                {
                    new KeyValuePair<string, int>(BannerColour.Black.ToString(), (int)BannerColour.Black),
                    new KeyValuePair<string, int>(BannerColour.White.ToString(), (int)BannerColour.White),
                    new KeyValuePair<string, int>(BannerColour.Grey.ToString(), (int)BannerColour.Grey),
                };
            }
        }

        public virtual ICollection<CreatePlanOptionViewModel> Options { get; set; }
    }
}
