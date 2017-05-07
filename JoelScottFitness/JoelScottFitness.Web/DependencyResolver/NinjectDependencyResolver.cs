using JoelScottFitness.Data.Modules;
using JoelScottFitness.PayPal.Modules;
using JoelScottFitness.Services.Modules;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.DependencyResolver
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            var modules = new NinjectModule[]
            {
                new DataModule(),
                new PayPalModule(),
                new ServiceModule()
            };

            kernel = new StandardKernel();
        }
        public object GetService(Type serviceType)
        {
            return kernel.Get(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}