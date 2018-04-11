using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Extensions;
using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
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

// TODO - see if can get Admin drop down to work on ipad

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
            var sectionImages = await jsfService.GetSectionImagesAsync();
            var kaleidoscopeImages = await jsfService.GetKaleidoscopeImagesAsync();
            var hallOfFame = await jsfService.GetHallOfFameEntriesAsync(true, 1);

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

            return RedirectToAction("PaymentOptions", "Home", new { customerId = customerResult.Result });
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

            return RedirectToAction("PaymentOptions", "Home", new { customerId = customerResult.Result });
        }

        [HttpGet]
        public ActionResult ConfirmOrder()
        {
            if (string.IsNullOrEmpty(Request.Params[SessionKeys.PayerId]))
                return RedirectToAction("Error", "Home", new { errorMessage = "Payer id null" });

            Session.Add(SessionKeys.PayerId, Request.Params[SessionKeys.PayerId]);

            var confirmOrderViewModel = (ConfirmOrderViewModel)Session[SessionKeys.ConfirmOrderViewModel];
            if (confirmOrderViewModel == null)
                return RedirectToAction("Error", "Home", new { errorMessage = "Confirm purchase view model null" });

            return View(confirmOrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddToBasket(long id)
        {
            var planOption = await jsfService.GetPlanOptionAsync(id);
            if (planOption == null)
            {
                // TODO maybe dont move to error but respond to ajax with error
                return RedirectToAction("Error", "Home", new { errorMessage = $"Failed to find item with id {id}." });
            }

            if (!basketHelper.AddItemToBasket(id, planOption.Name, planOption.Description, planOption.Price))
            {
                // TODO maybe dont move to error but respond to ajax with error
                return RedirectToAction("Error", "Home", new { errorMessage = $"Failed to add item wityh id {id} to basket." });
            }

            // TODO is this correct??
            return null;
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
            var basketItem = basketHelper.IncreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = new { Quantity = basketItem.Quantity }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DecreaseQuantity(long id)
        {
            var basketItem = basketHelper.DecreaseItemQuantity(id);

            return new JsonResult()
            {
                Data = new { Quantity = basketItem.Quantity }
            };
        }

        [HttpGet]
        public ActionResult GetBasketItemCount()
        {
            var basket = basketHelper.GetBasket();

            return new JsonResult()
            {
                Data = new
                {
                    items = basket.NumberOfItems,
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
                if (basketHelper.AddDiscountCode(discountCode))
                {
                    return new JsonResult()
                    {
                        Data = new
                        {
                            applied = true,
                            discountCodeId = discountCode.Id,
                            discount = discountCode.PercentDiscount,
                            description = $"{discountCode.PercentDiscount}% Discount!",
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
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
            basketHelper.RemoveDiscountCode();

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
            var basket = basketHelper.GetBasket();

            return new JsonResult()
            {
                Data = new
                {
                    TotalPrice = String.Format("{0:0.00}", basket.Total)
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult Basket()
        {
            var basket = basketHelper.GetBasket();

            return View(basket);
        }

        [HttpGet]
        public ActionResult PaymentOptions(Guid customerId)
        {
            if (customerId == null || customerId == Guid.Empty)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.CustomerIdNullErrorMessage });

            var paymentOptionViewModel = new PaymentOptionViewModel() { CustomerId = customerId };

            return View(paymentOptionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutWithPaypal(Guid customerId)
        {
            if (customerId == null || customerId == Guid.Empty)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.CustomerIdNullErrorMessage });

            // method used to initiate the paypal payment transaction
            string callbackUri = string.Format(Settings.Default.CallbackUri, RootUri);

            if (customerId == null || customerId == Guid.Empty)
                return RedirectToAction("Error", "Home", new { errorMessage = Settings.Default.CustomerIdNullErrorMessage });

            var basket = basketHelper.GetBasket();
            if (basket == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.BasketItemsNullErrorMessage, customerId) });

            var customerDetails = await jsfService.GetCustomerDetailsAsync(customerId);
            if (customerDetails == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.GetCustomerDetailsAsyncErrorMessage, customerId) });

            var confirmOrderViewModel = new ConfirmOrderViewModel()
            {
                CustomerDetails = customerDetails,
                Basket = basket,
            };

            var paymentInitiationResult = jsfService.InitiatePayPalPayment(confirmOrderViewModel, callbackUri);
            if (!paymentInitiationResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToInitiatePayPalPaymentErrorMessage, paymentInitiationResult.ErrorMessage) });

            Session.Add(SessionKeys.PaymentId, paymentInitiationResult.PaymentId);
            Session.Add(SessionKeys.TransactionId, paymentInitiationResult.TransactionId);

            confirmOrderViewModel.PayPalReference = paymentInitiationResult.PaymentId;
            confirmOrderViewModel.TransactionId = paymentInitiationResult.TransactionId;

            Session.Add(SessionKeys.ConfirmOrderViewModel, confirmOrderViewModel);

            return Redirect(paymentInitiationResult.PayPalRedirectUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToCompletePayPalPaymentErrorMessage, paymentResult.ErrorMessage) });
            }

            var confirmOrderViewModel = (ConfirmOrderViewModel)Session[SessionKeys.ConfirmOrderViewModel];
            if (confirmOrderViewModel == null)
                return RedirectToAction("Error", "Home", new { errorMessage = "Confirm purchase view model null" });

            confirmOrderViewModel.PurchaseStatus = OrderStatus.Complete;
            // save the pending purchase details in the database
            var savePurchaseResult = await jsfService.SaveOrderAsync(confirmOrderViewModel);
            if (!savePurchaseResult.Success)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToSaveItemsForPurchase, confirmOrderViewModel.CustomerDetails.EmailAddress) });

            // check whether the hall of fame is visible and re-add it after session is cleared
            var hallOfFameVisible = false;
            if (Session[SessionKeys.HallOfFame] != null && (bool)Session[SessionKeys.HallOfFame])
                hallOfFameVisible = true;

            // clear the users basket
            Session.Clear();

            // re-add this to the session
            Session[SessionKeys.HallOfFame] = hallOfFameVisible;

            var purchaseViewModel = await jsfService.GetOrderAsync(savePurchaseResult.Result);
            if (purchaseViewModel == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToRetrieveOrderErrorMessage, savePurchaseResult.Result, paymentCompletionResult.TransactionId) });

            // send confirmation email
            if (!await SendOrderConfirmationEmail(purchaseViewModel))
                logger.Error(string.Format(Settings.Default.FailedToSendOrderConfirmationEmailErrorMessage, paymentCompletionResult.TransactionId));

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
        public async Task<ActionResult> CustomerQuestionnaire(long orderId)
        {
            var questionnaireViewModel = new QuestionnaireViewModel();

            var purchase = await jsfService.GetOrderByOrderIdAsync(orderId);
            if (purchase == null)
            {
                ViewBag.Message = $"Oops! Order Id '{orderId}' not recognised, please ";
                return View(questionnaireViewModel);
            }

            if (purchase.QuestionnaireId.HasValue)
            {
                ViewBag.Message = $"Customer insight questionnaire for order Id '{orderId}' has already been submitted, to make amendments please ";
                return View(questionnaireViewModel);
            }

            questionnaireViewModel.OrderId = purchase.Id;

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
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToCreateOrUpdateQuestionnaireErrorMessage, questionnaire.OrderId) });

            ViewBag.Message = Settings.Default.QuestionnaireCompleteConfirmationMessage;

            return View(questionnaire);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.User)]
        public async Task<ActionResult> MyAccount()
        {
            var userId = User.Identity.Name;

            var customerDetails = await jsfService.GetCustomerDetailsAsync(userId);
            if (customerDetails == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToFindUserErrorMessage, userId) });

            var purchases = await jsfService.GetOrderSummaryAsync(customerDetails.Id);

            return View(purchases);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.User)]
        public async Task<ActionResult> MyPlans()
        {
            var userId = User.Identity.Name;

            var customerDetails = await jsfService.GetCustomerDetailsAsync(userId);
            if (customerDetails == null)
                return RedirectToAction("Error", "Home", new { errorMessage = string.Format(Settings.Default.FailedToFindUserErrorMessage, userId) });

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

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(CreateMessageViewModel messageViewModel)
        {
            if (!ModelState.IsValid)
                return View(messageViewModel);

            if (messageViewModel.JoinMailingList)
            {
                await UpdateMailingList(messageViewModel.EmailAddress);
            }

            var createMessageResult = await jsfService.CreateMessageAsync(messageViewModel);
            if (!createMessageResult.Success)
                return RedirectToAction("Error", "Home", new
                {
                    errorMessage = string.Format(Settings.Default.FailedToCreateMessageErrorMessage,
                                                                                            messageViewModel.Name,
                                                                                            messageViewModel.EmailAddress,
                                                                                            messageViewModel.Subject,
                                                                                            messageViewModel.Message)
                });

            if (!await SendMessageReceivedEmail(messageViewModel))
            {
                return RedirectToAction("Error", "Home", new
                {
                    errorMessage = string.Format(Settings.Default.FailedToSendMessageErrorMessage,
                                                                            messageViewModel.Name,
                                                                            messageViewModel.EmailAddress,
                                                                            messageViewModel.Subject,
                                                                            messageViewModel.Message)
                });
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> BeforeAndAfter(BeforeAndAfterViewModel model)
        {
            if (!ModelState.IsValid)
                return new JsonResult() { Data = new { success = false, errorMessage = Resources.GenericErrorMessage } };

            var beforeImageFilename = string.Format(Settings.Default.BeforeFileNameFormat, model.OrderId, Path.GetFileName(model.BeforeFile.FileName));
            var afterImageFilename = string.Format(Settings.Default.AfterFileNameFormat, model.OrderId, Path.GetFileName(model.AfterFile.FileName));

            var beforeUploadResult = fileHelper.UploadFile(model.BeforeFile, Settings.Default.HallOfFameDirectory, beforeImageFilename);
            var afterUploadResult = fileHelper.UploadFile(model.AfterFile, Settings.Default.HallOfFameDirectory, afterImageFilename);

            if (!beforeUploadResult.Success || !afterUploadResult.Success)
            {
                logger.Warn(string.Format(Settings.Default.FailedToUploadHallOfFameImagesErrorMessage, model.OrderId));
                return new JsonResult() { Data = new { success = false, errorMessage = Resources.GenericErrorMessage } };
            }

            if (!await jsfService.UploadHallOfFameAsync(model.OrderId, beforeUploadResult.UploadPath, afterUploadResult.UploadPath, model.Comment))
            {
                logger.Warn(string.Format(Settings.Default.FailedToUploadHallOfFameErrorMessage, model.OrderId));
                return new JsonResult() { Data = new { success = false, errorMessage = Resources.GenericErrorMessage } };
            }

            return new JsonResult() { Data = new { success = true } };
        }

        [HttpGet]
        public async Task<ActionResult> HallOfFame()
        {
            var hallOfFameEntries = await jsfService.GetHallOfFameEntriesAsync();

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

        private async Task<bool> SendOrderConfirmationEmail(OrderHistoryViewModel purchaseViewModel)
        {
            var email = this.RenderRazorViewToString("_OrderConfirmation", purchaseViewModel, RootUri);

            return await jsfService.SendEmailAsync(string.Format(Settings.Default.OrderConfirmation, purchaseViewModel.TransactionId), email, new List<string>() { purchaseViewModel.Customer.EmailAddress });
        }

        private PaymentCompletionResult GetPaymentCompletionResult()
        {
            var paymentCompletionResult = new PaymentCompletionResult() { Success = true };

            if (string.IsNullOrEmpty((string)Session[SessionKeys.PayerId]))
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

            //if (!((long?)Session[SessionKeys.PurchaseId]).HasValue)
            //{
            //    paymentCompletionResult.Success = false;
            //    logger.Warn(string.Format(Settings.Default.PaymentCompletionParameterNullErrorMessage, SessionKeys.PurchaseId));
            //}

            paymentCompletionResult.PayerId = (string)Session[SessionKeys.PayerId];
            paymentCompletionResult.PaymentId = (string)Session[SessionKeys.PaymentId];
            paymentCompletionResult.TransactionId = (string)Session[SessionKeys.TransactionId];
            //paymentCompletionResult.PurchaseId = ((long?)Session[SessionKeys.PurchaseId]).HasValue ? ((long?)Session[SessionKeys.PurchaseId]).Value : long.MinValue;

            return paymentCompletionResult;
        }

        private async Task<bool> SendMessageReceivedEmail(CreateMessageViewModel messageViewModel)
        {
            var email = this.RenderRazorViewToString("_EmailMessageReceived", messageViewModel, RootUri);

            return await jsfService.SendEmailAsync("New Customer Enquiry", email, new List<string>() { Settings.Default.JoelScottFitnessEmaillAddress });
        }
    }
}