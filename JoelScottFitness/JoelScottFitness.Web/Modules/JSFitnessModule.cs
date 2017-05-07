using System;
using Ninject.Modules;
using JoelScottFitness.Services.Services;

namespace JoelScottFitness.Web.Modules
{
    public class JSFitnessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJSFitnessService>().To<JSFitnessService>().InSingletonScope();
        }
    }
}