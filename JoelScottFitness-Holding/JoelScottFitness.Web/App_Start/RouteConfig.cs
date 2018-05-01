using System.Web.Mvc;
using System.Web.Routing;

namespace JoelScottFitness.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                  name: "CatchAll",
                  url: "{*any}",
                  defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
