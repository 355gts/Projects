using System.Collections.Generic;

namespace JoelScottFitness.Common.Models
{
    public class IndexViewModel
    {
        public IEnumerable<BlogViewModel> Blogs { get; set; }

        public IEnumerable<MediaViewModel> Videos { get; set; }
    }
}
