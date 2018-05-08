using JoelScottFitness.Web.Properties;
using System;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Filters
{
    public class RequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Settings.Default.RedirectEnabled)
            {
                var ukTime = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

                if (!filterContext.HttpContext.Request.Url.ToString().Contains(Settings.Default.TempUrl)
                    && TimeZoneInfo.ConvertTime(DateTime.UtcNow, ukTime) < Settings.Default.GoLiveDate
                    && !filterContext.HttpContext.Request.Url.ToString().Contains("Countdown"))
                    filterContext.Result = new RedirectResult(Settings.Default.RedirectUrl);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}