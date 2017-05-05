using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DiscountCodeViewModel dvm = new DiscountCodeViewModel()
            {
                Active = true,
                Code = "ABC123",
                Id = 123,
                PercentDiscount = 10,
                ValidFrom = DateTime.UtcNow.AddDays(-2),
                ValidTo = DateTime.UtcNow.AddDays(+3),
            };

            var mapper = new Mapper(Assembly.Load("JoelScottFitness.Services"));

            DiscountCode d = mapper.Map<DiscountCodeViewModel, DiscountCode>(dvm);
            d.ValidTo = DateTime.UtcNow.AddSeconds(-30);

            var t = mapper.Map<DiscountCode, DiscountCodeViewModel>(d);



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}