using JoelScottFitness.Web.Properties;
using log4net;
using System;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Filters
{
    public class RequestFilter : ActionFilterAttribute
    {
        ILog logger = LogManager.GetLogger(typeof(RequestFilter));

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookieExists = filterContext.HttpContext.Request.Cookies["access"] != null;
            if (!cookieExists && !filterContext.HttpContext.Request.Url.ToString().Contains("GetToken"))
            {
                if (Settings.Default.RedirectEnabled)
                {
                    var ukTime = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

                    if (TimeZoneInfo.ConvertTime(DateTime.UtcNow, ukTime) < Settings.Default.GoLiveDate
                        && !filterContext.HttpContext.Request.Url.ToString().Contains("Countdown"))
                    {
                        string redirectUrl = $"{filterContext.HttpContext.Request.Url.ToString().Trim('/')}{Settings.Default.RedirectUrl}";
                        filterContext.Result = new RedirectResult(redirectUrl);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}