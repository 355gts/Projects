using Ninject.Modules;

namespace JoelScottFitness.Data.Modules
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJSFitnessContext>().To<JSFitnessContext>();
            Bind<IJSFitnessRepository>().To<JSFitnessRespository>();
        }
    }
}
