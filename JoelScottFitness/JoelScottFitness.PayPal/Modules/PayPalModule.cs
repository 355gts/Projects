using JoelScottFitness.Common.Mapper;
using JoelScottFitness.PayPal.Services;
using Ninject.Modules;
using System.Reflection;

namespace JoelScottFitness.PayPal.Modules
{
    public class PayPalModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().To<Mapper>().Named("PayPalMapper")
                           .WithConstructorArgument("assemblyWithMappers", Assembly.Load("JoelScottFitness.PayPal"));
            Bind<IPayPalService>().To<PayPalService>();
        }
    }
}
