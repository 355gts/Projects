﻿using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Helpers;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.YouTube.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJSFitnessService jsfService;
        private readonly IHelper helper;
        private readonly IYouTubeClient youTubeClient;

        private const string basketKey = "Basket";

        public HomeController(IJSFitnessService jsfService, 
                              IHelper helper,
                              IYouTubeClient youTubeClient)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            if (helper == null)
                throw new ArgumentNullException(nameof(helper));

            if (youTubeClient == null)
                throw new ArgumentNullException(nameof(youTubeClient));

            this.jsfService = jsfService;
            this.helper = helper;
            this.youTubeClient = youTubeClient;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var blogs = await jsfService.GetBlogs(6);
            var videos = youTubeClient.GetVideos(3);

            var videoViewModel = videos.Select(v => new MediaViewModel()
            {
                VideoId = v.VideoId,
                Description = v.Description,
            });

            var indexViewModel = new IndexViewModel()
            {
                Blogs = blogs,
                Videos = videoViewModel,
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
            var videos = youTubeClient.GetVideos(10);

            var videoViewModel = videos.Select(v => new MediaViewModel()
            {
                VideoId = v.VideoId,
                Description = v.Description,
            });

            return View(videoViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Plans(Gender gender)
        {
            var plans = await jsfService.GetPlansByGender(gender);
            return View(plans);
        }

        [HttpGet]
        public ActionResult Lobby()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CustomerDetails","Home");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/CustomerDetails", showGuest = true });
            }
        }

        [HttpGet]
        public ActionResult CustomerDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AddToBasket(long id)
        {
            await AddItemToBasket(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void RemoveFromBasket(long id)
        {
            RemoveItemFromBasket(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult IncreaseQuantity(long id)
        {
            var itemQuantityModel = IncreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = itemQuantityModel
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DecreaseQuantity(long id)
        {
            var itemQuantityModel = DecreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = itemQuantityModel
            };
        }

        [HttpGet]
        public ActionResult GetBasketItemCount()
        {
            var numberOfItems = GetBasketItems().Count();

            return new JsonResult()
            {
                Data = new
                {
                    items = numberOfItems
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult CalculateTotal()
        {
            return new JsonResult()
            {
                Data = new
                {
                    TotalPrice = String.Format("{0:0.00}", CalculateTotalCost())
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> Basket()
        {
            var basket = GetBasketItems();
            
            var basketItems = await jsfService.GetBasketItems(basket.Keys.ToList());

            // map the quantities to the items
            foreach (var basketItem in basketItems)
            {
                basketItem.Quantity = basket.ContainsKey(basketItem.Id) ? basket[basketItem.Id].Quantity : 1;
            }
            
            return View(basketItems);
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

        private async Task AddItemToBasket(long id)
        {
            var basket = GetBasketItems();

            if (!basket.ContainsKey(id))
            {
                var planOption = await jsfService.GetPlanOptionAsync(id);

                if (planOption != null)
                {
                    basket.Add(id, new ItemQuantityViewModel()
                    {
                        Id = id,
                        Price = planOption.Price,
                        Quantity = 1, // 1 is the default quantity
                    });
                }
            }

            Session[basketKey] = basket;
        }

        private void RemoveItemFromBasket(long id)
        {
            var basket = GetBasketItems();

            if (basket.ContainsKey(id))
            {
                basket.Remove(id);
            }

            Session[basketKey] = basket;
        }

        private ItemQuantityViewModel IncreaseItemQuantity(long id)
        {
            var basket = GetBasketItems();

            if (basket.ContainsKey(id))
            {
                basket[id].Quantity = ++basket[id].Quantity;
                
                Session[basketKey] = basket;
            }

            return new ItemQuantityViewModel() { Id = id, Quantity = basket[id].Quantity };
        }

        private ItemQuantityViewModel DecreaseItemQuantity(long id)
        {
            var basket = GetBasketItems();

            // only permit the customer to decrease the quantity to 1
            if (basket.ContainsKey(id) && basket[id].Quantity > 1)
            {
                basket[id].Quantity = --basket[id].Quantity;

                Session[basketKey] = basket;
            }
            
            return new ItemQuantityViewModel() { Id = id, Quantity = basket[id].Quantity };
        }

        private IDictionary<long, ItemQuantityViewModel> GetBasketItems()
        {
            if (Session[basketKey] == null)
            {
                Session[basketKey] = new Dictionary<long, ItemQuantityViewModel>();
            }

            return (Dictionary<long, ItemQuantityViewModel>)Session[basketKey];
        }

        private double CalculateTotalCost()
        {
            double total = 0;

            var basket = GetBasketItems();

            foreach (var item in basket)
            {
                total += item.Value.Quantity * item.Value.Price;
            }

            return Math.Round(total, 2);
        }
    }
}