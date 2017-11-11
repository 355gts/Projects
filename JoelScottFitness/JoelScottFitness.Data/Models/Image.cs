using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class Image : BaseRecord
    {
        [Required]
        public string ImagePath { get; set; }
    }
}
