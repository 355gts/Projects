using JoelScottFitness.Web.Attributes;
using System.Web.Mvc;
#if !DEBUG
using JoelScottFitness.Web.Filters;
#endif

namespace JoelScottFitness.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionAttribute());
            //filters.Add(new RequestFilter());
#if !DEBUG
            filters.Add(new RequireSecureConnectionFilter());
#endif
        }
    }
}
