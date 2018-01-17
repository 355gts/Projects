using log4net;
using System.Web.Mvc;
using System.Web.Routing;

namespace JoelScottFitness.Web.Attributes
{
    public class GlobalExceptionAttribute : HandleErrorAttribute
    {
        private readonly ILog logger = LogManager.GetLogger(nameof(GlobalExceptionAttribute));

        public override void OnException(ExceptionContext filterContext)
        {
            logger.Error($"An error has occured '{filterContext.Exception.ToString()}'.");

            filterContext.ExceptionHandled = true;
            
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = "Error",
                controller = "Home",
            }));
        }
    }
}