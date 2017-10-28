using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Helpers;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.YouTube.Client;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IJSFitnessService jsfService;
        private readonly IHelper helper;
        private readonly IYouTubeClient youTubeClient;

        private const string basketKey = "Basket";

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
            this.SignInManager = _signInManager;
            this.UserManager = _userManager;
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
                return RedirectToAction("ExistingCustomerDetails","Home");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/ExistingCustomerDetails", showGuest = true });
            }
        }

        [HttpGet]
        public ActionResult NewCustomerDetails()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewCustomerDetails(CreateCustomerViewModel customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }
            
            var user = await jsfService.GetUser(customer.EmailAddress);

            if (customer.RegisterAccount)
            {
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "Email addresses is already registered");
                    customer.RegisterAccount = false;
                    customer.ConfirmEmailAddress = string.Empty;
                    customer.Password = string.Empty;
                    customer.ConfirmPassword = string.Empty;
                    return View(customer);
                }

                if (string.IsNullOrEmpty(customer.ConfirmEmailAddress) || customer.EmailAddress != customer.ConfirmEmailAddress)
                {
                    ModelState.AddModelError(string.Empty, "Email addresses must match to register for account");
                }

                if (string.IsNullOrEmpty(customer.Password) || customer.Password != customer.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Passwords must match to register for account");
                }

                if (!ModelState.IsValid)
                {
                    return View(customer);
                }
                
                if (user == null)
                {
                    var newUser = new AuthUser { UserName = customer.EmailAddress, Email = customer.EmailAddress };
                    var accountResult = await UserManager.CreateAsync(newUser, customer.Password);
                    if (accountResult.Succeeded)
                    {
                        await SignInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);
                        customer.UserId = newUser.Id;

                        await UserManager.AddToRoleAsync(newUser.Id, JsfRoles.AccountHolder);
                    }
                    else
                    {
                        accountResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e));

                        return View(customer);
                    }
                }
            }

            if (customer.JoinMailingList)
            {
                await UpdateMailingList(customer.EmailAddress);
            }

            var customerResult = await jsfService.CreateCustomer(customer);

            if (!customerResult.Success)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving customer details please try again.");
                return View(customer);
            }

            return RedirectToAction("ConfirmPurchase", "Home", new { customerId = customerResult.Result });
        }

        [HttpGet]
        public async Task<ActionResult> ExistingCustomerDetails()
        {
            if (User.Identity.IsAuthenticated)
            {
                var customerDetails = await jsfService.GetCustomerDetails(User.Identity.Name);
                //ensure this is defaulted to true
                customerDetails.JoinMailingList = true;

                return View(customerDetails);
            }

            return RedirectToAction("NewCustomerDetails", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExistingCustomerDetails(CustomerViewModel customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var user = await jsfService.GetUser(customer.EmailAddress);

            if (customer.JoinMailingList)
            {
                await UpdateMailingList(customer.EmailAddress);
            }

            var customerResult = await jsfService.UpdateCustomer(customer);

            if (!customerResult.Success)
            {
                ModelState.AddModelError(string.Empty, "An error occured saving customer details please try again.");
                return View(customer);
            }

            return RedirectToAction("ConfirmPurchase", "Home", new { customerId = customerResult.Result });
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmPurchase(long customerId)
        {
            var confirmPurchaseViewModel = new ConfirmPurchaseViewModel();

            var basket = GetBasketItems();

            var basketItems = await jsfService.GetBasketItems(basket.Keys.ToList());

            // map the quantities to the items
            foreach (var basketItem in basketItems)
            {
                basketItem.Quantity = basket.ContainsKey(basketItem.Id) ? basket[basketItem.Id].Quantity : 1;
            }

            confirmPurchaseViewModel.CustomerDetails = await jsfService.GetCustomerDetails(customerId);
            confirmPurchaseViewModel.BasketItems = basketItems;

            return View(confirmPurchaseViewModel);
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
        public async Task<ActionResult> Checkout(ConfirmPurchaseViewModel confirmPurchaseViewModel)
        {
            // method used to initiate the paypal payment transaction
            string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority +
                        "/Home/CompletePayment?";

            var plans = await jsfService.GetPlans();

            // map the plans back to the items
            confirmPurchaseViewModel.BasketItems.ToList().ForEach(i => i.Plan = plans.First(p => p.Id == i.PlanId));

            var paymentInitiationResult = jsfService.InitiatePayPalPayment(confirmPurchaseViewModel, baseUri);

            Session.Add("PaymentId", paymentInitiationResult.PaymentId);
            Session.Add("TransactionId", paymentInitiationResult.TransactionId);

            confirmPurchaseViewModel.PayPalReference = paymentInitiationResult.PaymentId;
            confirmPurchaseViewModel.TransactionId = paymentInitiationResult.TransactionId;

            // save the pending purchase details in the database
            var savePurchaseResult = await jsfService.SavePurchase(confirmPurchaseViewModel);
            
            if (!savePurchaseResult.Success)
            {
                // cancel the purchase
            }

            return Redirect(paymentInitiationResult.PayPalRedirectUrl);
        }

        [HttpGet]
        public async Task<ActionResult> CompletePayment()
        {
            // need to define a model to return with the success invoice number or failure reason
            string payerId = Request.Params["PayerID"];
            string paymentId = (string)Session["PaymentId"];
            string transactionId = (string)Session["TransactionId"];

            var paymentResult = jsfService.CompletePayPalPayment(paymentId, payerId);

            if (!paymentResult.Success)
            {
                await jsfService.UpdatePurchaseStatus(transactionId, PurchaseStatus.Failed);
                // return error
            }

            await jsfService.UpdatePurchaseStatus(transactionId, PurchaseStatus.Complete);

            // clear the users basket
            Session.Clear();

            // redirect them to a normal Get method incase they refresh
            return RedirectToAction("PaymentConfirmation", "Home", new { transactionId = transactionId });

        }

        public ActionResult PaymentConfirmation(string transactionId)
        {
            var completePaymentViewModel = new PaymentConfirmationViewModel()
            {
                TransactionId = transactionId,
            };

            // method used to complete the paypal payment transaction
            return View(completePaymentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> SubscribeToMailingList(string emailAddress)
        {
            return await UpdateMailingList(emailAddress);
        }

        [HttpGet]
        public async Task<ActionResult> CustomerQuestionnaire(string transactionId)
        {
            var purchaseId = await jsfService.GetPurchaseIdByTransactionId(transactionId);

            if (!purchaseId.HasValue)
            {
                // TBC - need to redirect to error view or something
                return View();
            }

            var questionnaire = new QuestionnaireViewModel()
            {
                PurchaseId = purchaseId.Value
            };

            return View(questionnaire);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerQuestionnaire(CreateQuestionnaireViewModel questionnaire)
        {
            if (ModelState.IsValid)
            {

            }

            return View(questionnaire);
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

        private async Task<bool> UpdateMailingList(string emailAddress)
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