using JoelScottFitness.YouTube.Client;
using Ninject.Modules;

namespace JoelScottFitness.YouTube.Modules
{
    public class YouTubeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IYouTubeClient>().To<YouTubeClient>();
        }
    }
}
