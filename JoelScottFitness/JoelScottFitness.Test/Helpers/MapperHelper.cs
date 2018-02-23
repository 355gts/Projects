using JoelScottFitness.Common.Mapper;
using System.Reflection;

namespace JoelScottFitness.Test.Helpers
{
    public static class MapperHelper
    {
        public static IMapper GetServiceMapper()
        {
            return new Mapper(Assembly.Load("JoelScottFitness.Services"));
        }

        public static IMapper GetPayPalMapper()
        {
            return new Mapper(Assembly.Load("JoelScottFitness.PayPal"));
        }
    }
}
