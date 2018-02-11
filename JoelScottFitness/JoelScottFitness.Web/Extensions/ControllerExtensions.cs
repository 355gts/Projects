using System.IO;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model, string rootUri)
        {
            controller.ViewData.Model = model;
            if (controller.TempData.ContainsKey("RootUri"))
                controller.TempData["RootUri"] = rootUri;
            else
                controller.TempData.Add("RootUri", rootUri);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}