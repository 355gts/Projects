using JoelScottFitness.YouTube.Models;
using System.Collections.Generic;

namespace JoelScottFitness.YouTube.Client
{
    public interface IYouTubeClient
    {
        IEnumerable<YouTubeVideo> GetVideos(int limit);
    }
}
