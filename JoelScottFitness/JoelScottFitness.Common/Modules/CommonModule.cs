using JoelScottFitness.Common.Helpers;
using Ninject.Modules;

namespace JoelScottFitness.Common.Modules
{
    public class CommonModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IHelper>().To<Helper>();
        }
    }
}
