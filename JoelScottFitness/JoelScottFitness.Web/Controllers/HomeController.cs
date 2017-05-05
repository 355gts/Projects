using JoelScottFitness.Common.Mapper;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Services.Services;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJSFitnessService jsfService;
        
        public HomeController(IJSFitnessService jsfService)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            this.jsfService = jsfService;
        }

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

        [HttpPost]
        public ActionResult Checkout()
        {
            // method used to initiate the paypal payment transaction
            //string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority +
            //            "/Home/CompletePayment?";

            //var paymentInitiationResult = jsfService.InitiatePayPalPayment(baseUri);

            //Session.Add("PaymentId", paymentInitiationResult.PaymentId);

            //return Redirect(paymentInitiationResult.PayPalRedirectUrl);

            return View();
        }

        [HttpPost]
        public ActionResult CompletePayment(string guid)
        {
            // method used to complete the paypal payment transaction
            return View();
        }
    }
}