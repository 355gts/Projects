using JoelScottFitness.Web.Helpers;
using Ninject.Modules;

namespace JoelScottFitness.Web.Modules
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBasketHelper>().To<BasketHelper>();
        }
    }
}