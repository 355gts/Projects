using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Services.Services;
using Ninject.Modules;
using System.Reflection;

namespace JoelScottFitness.Services.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().To<Mapper>().Named("ServiceMapper")
                           .WithConstructorArgument("assemblyWithMappers", Assembly.Load("JoelScottFitness.Services"));
            Bind<IJSFitnessService>().To<JSFitnessService>();
            Bind<IEmailService>().To<EmailService>();
        }
    }
}
