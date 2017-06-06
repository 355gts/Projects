using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Helpers;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJSFitnessService jsfService;
        private readonly IHelper helper;
        
        public HomeController(IJSFitnessService jsfService, 
                              IHelper helper)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            this.jsfService = jsfService;
            this.helper = helper;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var blogs = await jsfService.GetBlogs(6);

            var indexViewModel = new IndexViewModel()
            {
                Blogs = blogs
            };

            return View(indexViewModel);
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Blogs()
        {
            var blogs = await jsfService.GetBlogs();
            return View(blogs);
        }

        [HttpGet]
        public async Task<ActionResult> Blog(long id)
        {
            var blog = await jsfService.GetBlog(id);

            return new JsonResult() {
                Data = new
                {
                    title = blog.Title,
                    date = string.Format(blog.CreatedDate.ToString("dd{0} MMMM yyyy"), helper.GetSuffix(blog.CreatedDate.Day.ToString())),
                    subTitle = blog.SubHeader,
                    content = blog.Content,
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        [HttpGet]
        public ActionResult Media()
        {
            var mediaViewModel = new List<MediaViewModel>();
            return View(mediaViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Plans(Gender gender)
        {
            var plans = await jsfService.GetPlansByGender(gender);
            return View(plans);
        }

        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout()
        {
            // method used to initiate the paypal payment transaction
            string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority +
                        "/Home/CompletePayment?";

            var paymentInitiationResult = jsfService.InitiatePayPalPayment(baseUri);

            Session.Add("PaymentId", paymentInitiationResult.PaymentId);

            return Redirect(paymentInitiationResult.PayPalRedirectUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompletePayment(string guid)
        {
            // need to define a model to return with the success invoice number or failure reason
            string payerId = Request.Params["PayerID"];
            string paymentId = (string)Session["PaymentId"];

            var paymentResult = jsfService.CompletePayPalPayment(paymentId, payerId);

            if (!paymentResult.Success)
            {
                // return error
            }

            // method used to complete the paypal payment transaction
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> SubscribeToMailingList(string emailAddress)
        {
            var mailingListItemViewModel = new MailingListItemViewModel()
            {
                Active = true,
                Email = emailAddress,
            };

            return await jsfService.UpdateMailingList(mailingListItemViewModel);
        }
       
    }
}