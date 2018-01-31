using JoelScottFitness.Web.Helpers;
using Ninject.Modules;

namespace JoelScottFitness.Web.Modules
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            // TODO check scopes are correct when multiple users access the site
            Bind<IBasketHelper>().To<BasketHelper>().InSingletonScope();
        }
    }
}