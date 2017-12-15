using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Extensions;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Extensions;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IYouTubeClient youTubeClient;

        private const string basketKey = "Basket";

        private string errorMessage;

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
                              IYouTubeClient youTubeClient)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            if (youTubeClient == null)
                throw new ArgumentNullException(nameof(youTubeClient));

            this.jsfService = jsfService;
            this.youTubeClient = youTubeClient;
            this.SignInManager = _signInManager;
            this.UserManager = _userManager;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var blogs = await jsfService.GetBlogsAsync(6);
            var videos = youTubeClient.GetVideos(3);
            var sectionImages = await jsfService.GetSectionImages();
            var kaleidoscopeImages = await jsfService.GetKaleidoscopeImages();
            var hallOfFame = await jsfService.GetHallOfFameEntries(true, 1);

            if (sectionImages == null)
            {
                sectionImages = new SectionImageViewModel()
                {
                    SectionImage1 = Settings.Default.DefaultSectionImage1,
                    SectionImage2 = Settings.Default.DefaultSectionImage2,
                    SectionImage3 = Settings.Default.DefaultSectionImage3,
                    SplashImage = Settings.Default.DefaultSplashImage,
                };
            }

            var videoViewModel = videos.Select(v => new MediaViewModel()
            {
                VideoId = v.VideoId,
                Description = v.Description,
            });

            var indexViewModel = new IndexViewModel()
            {
                Blogs = blogs,
                Videos = videoViewModel,
                SectionImages = sectionImages,
                KaleidoscopeImages = kaleidoscopeImages,
                LatestHallOfFamer = hallOfFame.FirstOrDefault()
            };

            // used to determine whether to show the hall of fame link
            Session["HallOfFame"] = false;
            if (indexViewModel.LatestHallOfFamer != null)
                Session["HallOfFame"] = true;

            return View(indexViewModel);
        }

        [HttpGet]
        public ActionResult Countdown()
        {
            var goLive = new DateTime(2017, 12, 01, 18, 00, 00);
            DateTime nowTime = DateTime.Now;

            double result = (goLive - DateTime.Now).TotalSeconds;
            if (result <= 0)
                result += TimeSpan.FromHours(24).TotalSeconds;

            ViewBag.CountDownMilliseconds = result;

            return View();
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
            var blogs = await jsfService.GetBlogsAsync();
            return View(blogs.Where(b => b.Active).OrderByDescending(b => b.CreatedDate));
        }

        [HttpGet]
        public async Task<ActionResult> Blog(long id)
        {
            var blog = await jsfService.GetBlogAsync(id);

            return new JsonResult() {
                Data = new
                {
                    title = blog.Title,
                    date = blog.CreatedDate.DateTimeDisplayStringLong(),
                    subTitle = blog.SubHeader,
                    content = blog.Content,
                    images = blog.BlogImages,
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
            var plans = await jsfService.GetPlansByGenderAsync(gender);
            return View(plans.Where(p => p.Active).OrderByDescending(p => p.CreatedDate));
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
            
            var user = await jsfService.GetUserAsync(customer.EmailAddress);

            if (customer.RegisterAccount)
            {
                if (user != null)
                {
                    ModelState.AddModelError(string.Empty, "Email address is already registered");
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
                        // assign the user id to the customer account
                        customer.UserId = newUser.Id;
                        
                        await UserManager.AddToRoleAsync(newUser.Id, JsfRoles.User);

                        // TODO remove this logic when released
                        if (customer.EmailAddress.ToLower() == "blackmore__s@hotmail.com")
                        {
                            await UserManager.AddToRoleAsync(newUser.Id, JsfRoles.Admin);
                        }

                        // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code }, protocol: Request.Url.Scheme);

                        var callbackViewModel = new CallbackViewModel()
                        {
                            CallbackUrl = callbackUrl,
                        };

                        var email = this.RenderRazorViewToString("_EmailConfirmAccount", callbackViewModel);

                        await jsfService.SendEmailAsync("Joel Scott Fitness - Confirm Account", email, new List<string>() { newUser.Email });
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

            var customerResult = await jsfService.CreateCustomerAsync(customer);

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
                var customerDetails = await jsfService.GetCustomerDetailsAsync(User.Identity.Name);
                  
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

            var user = await jsfService.GetUserAsync(customer.EmailAddress);

            if (customer.JoinMailingList)
            {
                await UpdateMailingList(customer.EmailAddress);
            }

            var customerResult = await jsfService.UpdateCustomerAsync(customer);

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

            var basketItems = await jsfService.GetBasketItemsAsync(basket.Keys.ToList());

            // map the quantities to the items
            foreach (var basketItem in basketItems)
            {
                basketItem.Quantity = basket.ContainsKey(basketItem.Id) ? basket[basketItem.Id].Quantity : 1;
            }

            confirmPurchaseViewModel.CustomerDetails = await jsfService.GetCustomerDetailsAsync(customerId);
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
            
            var basketItems = await jsfService.GetBasketItemsAsync(basket.Keys.ToList());

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

            var plans = await jsfService.GetPlansAsync();

            // map the plans back to the items
            confirmPurchaseViewModel.BasketItems.ToList().ForEach(i => i.Plan = plans.First(p => p.Id == i.PlanId));

            var paymentInitiationResult = jsfService.InitiatePayPalPayment(confirmPurchaseViewModel, baseUri);

            Session.Add("PaymentId", paymentInitiationResult.PaymentId);
            Session.Add("TransactionId", paymentInitiationResult.TransactionId);

            confirmPurchaseViewModel.PayPalReference = paymentInitiationResult.PaymentId;
            confirmPurchaseViewModel.TransactionId = paymentInitiationResult.TransactionId;

            // save the pending purchase details in the database
            var savePurchaseResult = await jsfService.SavePurchaseAsync(confirmPurchaseViewModel);
            
            if (!savePurchaseResult.Success)
            {
                // cancel the purchase
            }
            Session.Add("PurchaseId", savePurchaseResult.Result);

            return Redirect(paymentInitiationResult.PayPalRedirectUrl);
        }

        [HttpGet]
        public async Task<ActionResult> CompletePayment()
        {
            // need to define a model to return with the success invoice number or failure reason
            string payerId = Request.Params["PayerID"];
            string paymentId = (string)Session["PaymentId"];
            string transactionId = (string)Session["TransactionId"];
            long purchaseId = (long)Session["PurchaseId"];

            var paymentResult = jsfService.CompletePayPalPayment(paymentId, payerId);

            if (!paymentResult.Success)
            {
                await jsfService.UpdatePurchaseStatusAsync(transactionId, PurchaseStatus.Failed);
                // return error
            }

            await jsfService.UpdatePurchaseStatusAsync(transactionId, PurchaseStatus.Complete);

            // check whether the hall of fame is visible and re-add it after session is cleared
            var hallOfFameVisible = false;
            if (Session["HallOfFame"] != null && (bool)Session["HallOfFame"])
                hallOfFameVisible = true;

            // clear the users basket
            Session.Clear();

            // re-add this to the session
            Session["HallOfFame"] = hallOfFameVisible;

            var purchaseViewModel = await jsfService.GetPurchaseAsync(purchaseId);

            // send confirmation email
            var email = this.RenderRazorViewToString("_OrderConfirmation", purchaseViewModel);
            
            await jsfService.SendEmailAsync($"Joel Scott Fitness - Order #{purchaseViewModel.TransactionId} Confirmation", email, new List<string>() { "Blackmore__s@hotmail.com" });
            
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
            var purchase = await jsfService.GetPurchaseByTransactionIdAsync(transactionId);
            if (purchase == null)
            {
                ViewBag.Message = $"Oops! Transaction Id '{transactionId}' not recognised, please contact customerservice@JoelScottFitness.com.";
                return View();
            }

            if (purchase.QuestionnaireId.HasValue)
            {
                ViewBag.Message = $"Customer insight questionnaire for transaction Id '{transactionId}' has already been submitted, to make amendments please contact customerservice@JoelScottFitness.com.";
                return View();
            }

            var questionnaireViewModel = new QuestionnaireViewModel()
            {
                PurchaseId = purchase.Id
            };

            return View(questionnaireViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CustomerQuestionnaire(QuestionnaireViewModel questionnaire)
        {
            if (!ModelState.IsValid)
                return View(questionnaire);

            var questionnaireResult = await jsfService.CreateOrUpdateQuestionnaireAsync(questionnaire);

            if (!questionnaireResult.Success)
            {
                //errorMessage = "Oh ohhh, damn it!";
                return RedirectToAction("Error", "Home");
            }

            ViewBag.Message = $"Thanks, your tailored workout plan will be with you in the next 24 hours.";

            return View();
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.User)]
        public async Task<ActionResult> MyAccount()
        {
            var userId = User.Identity.Name;

            var customerDetails = await jsfService.GetCustomerDetailsAsync(userId);

            // TODO handle this error i.e. no user
            if (customerDetails == null)
                return RedirectToAction("Error", "Home");

            var purchases = await jsfService.GetPurchaseSummaryAsync(customerDetails.Id);

            return View(purchases);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.User)]
        public async Task<ActionResult> MyPlans()
        {
            var userId = User.Identity.Name;

            var customerDetails = await jsfService.GetCustomerDetailsAsync(userId);

            // TODO handle this error i.e. no user
            if (customerDetails == null)
                return RedirectToAction("Error", "Home");

            var purchase = await jsfService.GetCustomerPlansAsync(customerDetails.Id);

            return View(purchase);
        }

        [HttpGet]
        public ActionResult Error()
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewBag.Message = errorMessage;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BeforeAndAfter(BeforeAndAfterViewModel model)
        {
            var beforeImageFilename = $"{model.PurchasedItemId}_BEFORE_{Path.GetFileName(model.BeforeFile.FileName)}";
            var afterImageFilename = $"{model.PurchasedItemId}_AFTER_{Path.GetFileName(model.AfterFile.FileName)}";

            var beforeUploadPath = SaveFile(model.BeforeFile, "Content/Images/HallOfFame", beforeImageFilename);
            var afterUploadPath = SaveFile(model.AfterFile, "Content/Images/HallOfFame", afterImageFilename);

            var result = await jsfService.UploadHallOfFameAsync(model.PurchasedItemId, beforeUploadPath, afterUploadPath, model.Comment);

            return RedirectToAction("MyPlans", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> HallOfFame()
        {
            var hallOfFameEntries = await jsfService.GetHallOfFameEntries();

            return View(hallOfFameEntries);
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            var model = new CallbackViewModel()
            {
                CallbackUrl = "https://www.JoelScottFitness.com/Account/ResetPassword?userEmail=blah",
            };

            return View(model);
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

            return await jsfService.UpdateMailingListAsync(mailingListItemViewModel);
        }

        private string SaveFile(HttpPostedFileBase postedFile, string directory, string name = null)
        {
            string uploadPath = null;
            try
            {
                string path = Server.MapPath($"~/{directory}/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = !string.IsNullOrEmpty(name)
                                    ? name
                                    : Path.GetFileName(postedFile.FileName);

                postedFile.SaveAs(path + fileName);
                uploadPath = $"/{directory}/{fileName}";
            }
            catch (Exception ex)
            {
                // TODO log exception
            }

            return uploadPath;
        }
    }
}