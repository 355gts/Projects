using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Data.Models
{
    public class BaseRecord
    {
        [Key]
        public long Id { get; set; }
    }
}
