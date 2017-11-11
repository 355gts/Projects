using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JoelScottFitness.Common.Models
{
    [DataContract]
    public class ImageListViewModel
    {
        public IEnumerable<ImageViewModel> Images { get; set; }
    }
}
