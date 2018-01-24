using JoelScottFitness.Web.Attributes;
using JoelScottFitness.Web.Filters;
using System.Web.Mvc;

namespace JoelScottFitness.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionAttribute());
#if !DEBUG
            filters.Add(new RequireSecureConnectionFilter());
#endif
        }
    }
}
