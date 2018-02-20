using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Extensions;
using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Constants;
using JoelScottFitness.Web.Extensions;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HomeController));

        private readonly IJSFitnessService jsfService;
        private readonly IYouTubeClient youTubeClient;
        private readonly IBasketHelper basketHelper;
        private readonly IFileHelper fileHelper;

        public string RootUri { get { return $"{Request.Url.Scheme}://{Request.Url.Authority}"; } }

        public HomeController(IJSFitnessService jsfService,
                              IYouTubeClient youTubeClient,
                              IBasketHelper basketHelper,
                              IFileHelper fileHelper)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            if (youTubeClient == null)
                throw new ArgumentNullException(nameof(youTubeClient));

            if (basketHelper == null)
                throw new ArgumentNullException(nameof(basketHelper));

            if (fileHelper == null)
                throw new ArgumentNullException(nameof(fileHelper));

            this.jsfService = jsfService;
            this.youTubeClient = youTubeClient;
            this.basketHelper = basketHelper;
            this.fileHelper = fileHelper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(bool christmas = false)
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
            Session[SessionKeys.HallOfFame] = false;
            if (indexViewModel.LatestHallOfFamer != null)
                Session[SessionKeys.HallOfFame] = true;

            if (christmas)
                Session[SessionKeys.Christmas] = true;

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

            if (blogs == null || !blogs.Any())
                return View();

            return View(blogs.Where(b => b.Active).OrderByDescending(b => b.CreatedDate));
        }

        [HttpGet]
        public async Task<ActionResult> Blog(long id)
        {
            var blog = await jsfService.GetBlogAsync(id);

            return new JsonResult()
            {
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
        public async Task<ActionResult> Plans(Gender gender = Gender.Male)
        {
            var plans = await jsfService.GetPlansByGenderAsync(gender);
            return View(plans.Where(p => p.Active).OrderByDescending(p => p.CreatedDate));
        }

        [HttpGet]
        public ActionResult Lobby()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ExistingCustomerDetails", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/Home/ExistingCustomerDetails", showGuest = true });
            }
        }

        [HttpGet]
        public ActionResult GuestDetails()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GuestDetails(CreateCustomerViewModel customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            var customerResult = await jsfService.CreateCustomerAsync(customer);
            if (!customerResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.CreateGuestDetailsFailedErrorMessage, customer.EmailAddress) });

            if (customer.JoinMailingList)
            {
                await UpdateMailingList(customer.EmailAddress);
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

            return RedirectToAction("Login", "Account", new { ReturnUrl = @"/Home/ExistingCustomerDetails" });
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
            if (user == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.UnableToFindExistingCustomerErrorMessage, customer.EmailAddress) });

            if (customer.JoinMailingList)
            {
                await UpdateMailingList(customer.EmailAddress);
            }

            var customerResult = await jsfService.UpdateCustomerAsync(customer);
            if (!customerResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToUpdateExistingCustomerDetailsErrorMessage, customer.EmailAddress) });

            return RedirectToAction("ConfirmPurchase", "Home", new { customerId = customerResult.Result });
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmPurchase(Guid customerId)
        {
            var confirmPurchaseViewModel = new ConfirmPurchaseViewModel();

            if (customerId == null || customerId == Guid.Empty)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.CustomerIdNullErrorMessage });

            var basket = basketHelper.GetBasketItems();
            if (basket == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.BasketItemsNullErrorMessage, customerId) });

            var basketItems = await jsfService.GetBasketItemsAsync(basket.Keys.ToList());
            if (basketItems == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.BasketItemsAsyncNullErrorMessage, customerId, string.Join(",", basket.Keys.ToList())) });

            // map the quantities to the items
            foreach (var basketItem in basketItems)
            {
                basketItem.Quantity = basket.ContainsKey(basketItem.Id) ? basket[basketItem.Id].Quantity : Settings.Default.DefaultItemQuantity;
            }

            var customerDetails = await jsfService.GetCustomerDetailsAsync(customerId);
            if (customerDetails == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.GetCustomerDetailsAsyncErrorMessage, customerId) });

            confirmPurchaseViewModel.CustomerDetails = customerDetails;
            confirmPurchaseViewModel.BasketItems = basketItems;

            if (Session[SessionKeys.DiscountCode] != null)
            {
                confirmPurchaseViewModel.DiscountCodeId = ((DiscountCodeViewModel)Session[SessionKeys.DiscountCode]).Id;
                confirmPurchaseViewModel.DiscountCode = (DiscountCodeViewModel)Session[SessionKeys.DiscountCode];
            }

            return View(confirmPurchaseViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task AddToBasket(long id)
        {
            await basketHelper.AddItemToBasket(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void RemoveFromBasket(long id)
        {
            basketHelper.RemoveItemFromBasket(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult IncreaseQuantity(long id)
        {
            var itemQuantityModel = basketHelper.IncreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = itemQuantityModel
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DecreaseQuantity(long id)
        {
            var itemQuantityModel = basketHelper.DecreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = itemQuantityModel
            };
        }

        [HttpGet]
        public ActionResult GetBasketItemCount()
        {
            var numberOfItems = basketHelper.GetBasketItems().Count();

            return new JsonResult()
            {
                Data = new
                {
                    items = numberOfItems
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ApplyDiscountCode(string code)
        {
            var discountCode = await jsfService.GetDiscountCodeAsync(code);

            if (discountCode != null && discountCode.Active)
            {
                Session[SessionKeys.DiscountCode] = discountCode;

                return new JsonResult()
                {
                    Data = new
                    {
                        applied = Session[SessionKeys.DiscountCode] != null,
                        discountCodeId = discountCode.Id,
                        discount = discountCode.PercentDiscount,
                        description = $"{discountCode.PercentDiscount}% Discount!",
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new JsonResult()
            {
                Data = new
                {
                    Applied = false
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoveDiscountCode()
        {
            if (Session[SessionKeys.DiscountCode] != null)
                Session.Remove(SessionKeys.DiscountCode);

            return new JsonResult()
            {
                Data = new
                {
                    Applied = false
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
                    TotalPrice = String.Format("{0:0.00}", basketHelper.CalculateTotalCost())
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public async Task<ActionResult> Basket()
        {
            var basket = basketHelper.GetBasketItems();

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
            if (confirmPurchaseViewModel == null)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.ConfirmPurchaseViewModelNullErrorMessage });

            // method used to initiate the paypal payment transaction
            string callbackUri = string.Format(Settings.Default.CallbackUri, RootUri);

            var plans = await jsfService.GetPlansAsync();
            if (plans == null)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.FailedToRetrievePlansErrorMessage });

            DiscountCodeViewModel discountCodeViewModel = null;
            if (confirmPurchaseViewModel.DiscountCodeId.HasValue)
            {
                discountCodeViewModel = await jsfService.GetDiscountCodeAsync(confirmPurchaseViewModel.DiscountCodeId.Value);
            }

            // map the plans back to the items
            confirmPurchaseViewModel.BasketItems.ToList().ForEach(i =>
            {
                i.Plan = plans.First(p => p.Id == i.PlanId);
                if (discountCodeViewModel != null)
                {
                    i.Price = Math.Round(i.Price - (i.Price / 100 * discountCodeViewModel.PercentDiscount), 2);
                }
            });

            var paymentInitiationResult = jsfService.InitiatePayPalPayment(confirmPurchaseViewModel, callbackUri);
            if (!paymentInitiationResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToInitiatePayPalPaymentErrorMessage, paymentInitiationResult.ErrorMessage) });

            Session.Add(SessionKeys.PaymentId, paymentInitiationResult.PaymentId);
            Session.Add(SessionKeys.TransactionId, paymentInitiationResult.TransactionId);

            confirmPurchaseViewModel.PayPalReference = paymentInitiationResult.PaymentId;
            confirmPurchaseViewModel.TransactionId = paymentInitiationResult.TransactionId;

            // save the pending purchase details in the database
            var savePurchaseResult = await jsfService.SavePurchaseAsync(confirmPurchaseViewModel);
            if (!savePurchaseResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToSaveItemsForPurchase, confirmPurchaseViewModel.CustomerDetails.EmailAddress) });

            Session.Add(SessionKeys.PurchaseId, savePurchaseResult.Result);

            return Redirect(paymentInitiationResult.PayPalRedirectUrl);
        }

        [HttpGet]
        public async Task<ActionResult> CompletePayment()
        {
            // retrieve parameters from request to complete payment
            var paymentCompletionResult = GetPaymentCompletionResult();
            if (!paymentCompletionResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.PaymentCompletionErrorMessage });

            // complete the paypal payment
            var paymentResult = jsfService.CompletePayPalPayment(paymentCompletionResult.PaymentId, paymentCompletionResult.PayerId);
            if (!paymentResult.Success)
            {
                await jsfService.UpdatePurchaseStatusAsync(paymentCompletionResult.TransactionId, PurchaseStatus.Failed);
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToCompletePayPalPaymentErrorMessage, paymentResult.ErrorMessage) });
            }

            // update the purchase to complete
            if (!await jsfService.UpdatePurchaseStatusAsync(paymentCompletionResult.TransactionId, PurchaseStatus.Complete))
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToUpdatePurchaseStatusErrorMessage, paymentCompletionResult.TransactionId) });

            // check whether the hall of fame is visible and re-add it after session is cleared
            var hallOfFameVisible = false;
            if (Session[SessionKeys.HallOfFame] != null && (bool)Session[SessionKeys.HallOfFame])
                hallOfFameVisible = true;

            // clear the users basket
            Session.Clear();

            // re-add this to the session
            Session[SessionKeys.HallOfFame] = hallOfFameVisible;

            var purchaseViewModel = await jsfService.GetPurchaseAsync(paymentCompletionResult.PurchaseId);
            if (purchaseViewModel == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToRetrievePurchaseErrorMessage, paymentCompletionResult.PurchaseId, paymentCompletionResult.TransactionId) });

            // send confirmation email
            if (!await SendOrderConfirmationEmail(purchaseViewModel))
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToSendOrderConfirmationEmailErrorMessage, paymentCompletionResult.TransactionId) });

            // redirect them to a normal Get method incase they refresh
            return RedirectToAction("PaymentConfirmation", "Home", new { transactionId = paymentCompletionResult.TransactionId });
        }

        [HttpGet]
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
            var questionnaireViewModel = new QuestionnaireViewModel();

            var purchase = await jsfService.GetPurchaseByTransactionIdAsync(transactionId);
            if (purchase == null)
            {
                ViewBag.Message = $"Oops! Transaction Id '{transactionId}' not recognised, please contact customerservice@JoelScottFitness.com.";
                return View(questionnaireViewModel);
            }

            if (purchase.QuestionnaireId.HasValue)
            {
                ViewBag.Message = $"Customer insight questionnaire for transaction Id '{transactionId}' has already been submitted, to make amendments please contact customerservice@JoelScottFitness.com.";
                return View(questionnaireViewModel);
            }

            questionnaireViewModel.PurchaseId = purchase.Id;

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
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToCreateOrUpdateQuestionnaireErrorMessage, questionnaire.PurchaseId) });

            ViewBag.Message = Settings.Default.QuestionnaireCompleteConfirmationMessage;

            return View(questionnaire);
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
        public ActionResult Error(string errorMessage = null)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                logger.Warn($"An error has occured, error details: '{errorMessage}'.");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BeforeAndAfter(BeforeAndAfterViewModel model)
        {
            if (string.IsNullOrEmpty(model.AfterFile.FileName) || model.AfterFile.ContentLength == 0)
                ModelState.AddModelError(string.Empty, string.Format(Settings.Default.ImageUploadErrorMessage, "After Image"));

            if (string.IsNullOrEmpty(model.BeforeFile.FileName) || model.BeforeFile.ContentLength == 0)
                ModelState.AddModelError(string.Empty, string.Format(Settings.Default.ImageUploadErrorMessage, "Before Image"));

            if (string.IsNullOrEmpty(model.Comment))
                ModelState.AddModelError(string.Empty, Settings.Default.MissingCommentErrorMessage);

            if (!ModelState.IsValid)
                return View(model);

            var beforeImageFilename = string.Format(Settings.Default.BeforeFileNameFormat, model.PurchasedItemId, Path.GetFileName(model.BeforeFile.FileName));
            var afterImageFilename = string.Format(Settings.Default.AfterFileNameFormat, model.PurchasedItemId, Path.GetFileName(model.AfterFile.FileName));

            var beforeUploadResult = fileHelper.UploadFile(model.BeforeFile, Settings.Default.HallOfFameDirectory, beforeImageFilename);
            var afterUploadResult = fileHelper.UploadFile(model.AfterFile, Settings.Default.HallOfFameDirectory, afterImageFilename);

            if (!beforeUploadResult.Success || !afterUploadResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToUploadHallOfFameImagesErrorMessage, model.PurchasedItemId) });

            if (!await jsfService.UploadHallOfFameAsync(model.PurchasedItemId, beforeUploadResult.UploadPath, afterUploadResult.UploadPath, model.Comment))
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToUploadHallOfFameErrorMessage, model.PurchasedItemId) });

            return RedirectToAction("MyPlans", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> HallOfFame()
        {
            var hallOfFameEntries = await jsfService.GetHallOfFameEntries();

            return View(hallOfFameEntries);
        }

        private async Task<bool> UpdateMailingList(string emailAddress)
        {
            try
            {
                var mailingListItemViewModel = new MailingListItemViewModel()
                {
                    Active = true,
                    Email = emailAddress,
                };

                return await jsfService.UpdateMailingListAsync(mailingListItemViewModel);
            }
            catch (Exception ex)
            {
                logger.Warn($"An error occurred attempting to add '{emailAddress}' to mailing list, error details: '{ex.Message}.");
                return false;
            }
        }

        private async Task<bool> SendOrderConfirmationEmail(PurchaseHistoryViewModel purchaseViewModel)
        {
            var email = this.RenderRazorViewToString("_OrderConfirmation", purchaseViewModel, RootUri);

            return await jsfService.SendEmailAsync(string.Format(Settings.Default.PurchaseConfirmation, purchaseViewModel.TransactionId), email, new List<string>() { purchaseViewModel.Customer.EmailAddress });
        }

        private PaymentCompletionResult GetPaymentCompletionResult()
        {
            var paymentCompletionResult = new PaymentCompletionResult() { Success = true };

            if (string.IsNullOrEmpty(Request.Params[SessionKeys.PayerId]))
            {
                paymentCompletionResult.Success = false;
                logger.Warn(string.Format(Settings.Default.PaymentCompletionParameterNullErrorMessage, SessionKeys.PayerId));
            }

            if (string.IsNullOrEmpty((string)Session[SessionKeys.PaymentId]))
            {
                paymentCompletionResult.Success = false;
                logger.Warn(string.Format(Settings.Default.PaymentCompletionParameterNullErrorMessage, SessionKeys.PaymentId));
            }

            if (string.IsNullOrEmpty((string)Session[SessionKeys.TransactionId]))
            {
                paymentCompletionResult.Success = false;
                logger.Warn(string.Format(Settings.Default.PaymentCompletionParameterNullErrorMessage, SessionKeys.TransactionId));
            }

            if (!((long?)Session[SessionKeys.PurchaseId]).HasValue)
            {
                paymentCompletionResult.Success = false;
                logger.Warn(string.Format(Settings.Default.PaymentCompletionParameterNullErrorMessage, SessionKeys.PurchaseId));
            }

            paymentCompletionResult.PayerId = Request.Params[SessionKeys.PayerId];
            paymentCompletionResult.PaymentId = (string)Session[SessionKeys.PaymentId];
            paymentCompletionResult.TransactionId = (string)Session[SessionKeys.TransactionId];
            paymentCompletionResult.PurchaseId = ((long?)Session[SessionKeys.PurchaseId]).HasValue ? ((long?)Session[SessionKeys.PurchaseId]).Value : long.MinValue;

            return paymentCompletionResult;
        }
    }
}