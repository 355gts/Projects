using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Extensions;
using JoelScottFitness.Web.Properties;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AdminController));

        private readonly IJSFitnessService jsfService;
        private readonly IFileHelper fileHelper;

        public string RootUri { get { return $"{Request.Url.Scheme}://{Request.Url.Authority}"; } }

        public AdminController(IJSFitnessService jsfService,
                               IFileHelper fileHelper)
        {
            if (jsfService == null)
                throw new ArgumentNullException(nameof(jsfService));

            if (fileHelper == null)
                throw new ArgumentNullException(nameof(fileHelper));

            this.jsfService = jsfService;
            this.fileHelper = fileHelper;
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> Blogs()
        {
            var blogs = await jsfService.GetBlogsAsync();

            return View(blogs.OrderByDescending(b => b.CreatedDate));
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> PreviewBlog(long blogId)
        {
            var blog = await jsfService.GetBlogAsync(blogId);

            if (blog == null)
                return RedirectToAction("Blogs", "Admin");

            return View(new List<BlogViewModel>() { blog });
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public ActionResult CreateBlog()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetToken()
        {
            var cookie = new HttpCookie("access");
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.UtcNow.AddDays(1);
            HttpContext.Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CreateBlog(CreateBlogViewModel blog, HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
                ModelState.AddModelError("NoImage", "You must select an image");

            if (blog.BlogImages != null && blog.BlogImages.Any(b => b.PostedFile == null))
                ModelState.AddModelError("NoBlogImages", "Please select an image for each entry");

            if (ModelState.IsValid)
            {
                var uploadResult = UploadFile(postedFile, Settings.Default.BlogImageDirectory);
                if (!uploadResult.Success)
                {
                    return View(blog);
                }
                blog.ImagePath = uploadResult.UploadPath;

                if (blog.BlogImages != null && blog.BlogImages.Any())
                {
                    foreach (var blogImage in blog.BlogImages)
                    {
                        uploadResult = UploadFile(blogImage.PostedFile, Settings.Default.BlogImageDirectory);
                        if (!uploadResult.Success)
                        {
                            return View(blog);
                        }
                        blogImage.ImagePath = uploadResult.UploadPath;
                    }
                }

                var result = await jsfService.CreateBlogAsync(blog);

                if (result.Success)
                    return RedirectToAction("Blogs", "Admin");
            }

            return View(blog);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateBlog(long blogId)
        {
            var blog = await jsfService.GetBlogAsync(blogId);

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateBlog(BlogViewModel blog, HttpPostedFileBase postedFile)
        {
            if (blog.BlogImages != null && blog.BlogImages.Any(b => (b.PostedFile == null && string.IsNullOrEmpty(b.ImagePath))))
                ModelState.AddModelError("NoBlogImages", "Please select an image for new entries");

            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    var uploadResult = fileHelper.UploadFile(postedFile, Settings.Default.BlogImageDirectory);
                    if (!uploadResult.Success)
                    {
                        return View(blog);
                    }
                    blog.ImagePath = uploadResult.UploadPath;
                }

                if (blog.BlogImages != null && blog.BlogImages.Any())
                {
                    foreach (var blogImage in blog.BlogImages.Where(i => i.PostedFile != null).ToList())
                    {
                        var uploadResult = UploadFile(blogImage.PostedFile, Settings.Default.BlogImageDirectory);
                        if (!uploadResult.Success)
                        {
                            return View(blog);
                        }
                        blogImage.ImagePath = uploadResult.UploadPath;
                    }
                }

                var result = await jsfService.UpdateBlogAsync(blog);

                if (result.Success)
                    return RedirectToAction("Blogs", "Admin");
            }

            return View(blog);
        }

        [HttpPost]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DeleteBlog(long blogId)
        {
            var success = await jsfService.DeleteBlogAsync(blogId);

            if (!success)
                logger.Warn($"Failed to delete blog with id {blogId}.");

            return RedirectToAction("Blogs", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<JsonResult> DeleteBlogImage(long blogId, long blogImageId)
        {
            var success = await jsfService.DeleteBlogImageAsync(blogImageId);

            if (!success)
                logger.Warn($"Failed to delete blog image with id {blogImageId}.");

            return new JsonResult()
            {
                Data = new
                {
                    success = true
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> Plans()
        {
            var plans = await jsfService.GetPlansAsync();

            return View(plans.OrderByDescending(p => p.CreatedDate));
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public ActionResult CreatePlan()
        {
            return View(new CreatePlanViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CreatePlan(CreatePlanViewModel plan, HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
                ModelState.AddModelError("NoImage", "You must select an image");

            if (ModelState.IsValid)
            {
                var uploadResult = UploadFile(postedFile, Settings.Default.PlanImageDirectory);
                if (!uploadResult.Success)
                {
                    return View(plan);
                }

                // map the image path and name to the plan and options
                plan.ImagePathLarge = uploadResult.UploadPath;
                foreach (var planOption in plan.Options)
                {
                    planOption.ImagePath = uploadResult.UploadPath;
                    planOption.Name = plan.Name;
                }

                var result = await jsfService.CreatePlanAsync(plan);

                if (result.Success)
                    return RedirectToAction("Plans", "Admin");
            }

            return View(plan);
        }



        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> PreviewPlan(long planId)
        {
            var plan = await jsfService.GetUiPlanAsync(planId);

            if (plan == null)
                return RedirectToAction("Plans", "Admin");

            return View(new List<UiPlanViewModel>() { plan });
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdatePlan(long planId)
        {
            var plan = await jsfService.GetPlanAsync(planId);

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdatePlan(PlanViewModel plan, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    var uploadResult = UploadFile(postedFile, Settings.Default.PlanImageDirectory);
                    if (!uploadResult.Success)
                    {
                        return View(plan);
                    }

                    plan.ImagePathLarge = uploadResult.UploadPath;

                }

                // map the image path and plan name to the plan and options
                foreach (var planOption in plan.Options)
                {
                    planOption.ImagePath = plan.ImagePathLarge;
                    planOption.Name = plan.Name;
                }

                var result = await jsfService.UpdatePlanAsync(plan);

                if (result.Success)
                    return RedirectToAction("Plans", "Admin");
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateBlogStatus(long blogId, bool status)
        {
            var result = await jsfService.UpdateBlogStatusAsync(blogId, status);

            if (result)
                return RedirectToAction("Blogs", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdatePlanStatus(long planId, bool status)
        {
            var result = await jsfService.UpdatePlanStatusAsync(planId, status);

            if (result)
                return RedirectToAction("Plans", "Admin");

            return View();
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DiscountCodes()
        {
            var discountCodes = await jsfService.GetDiscountCodesAsync();

            return View(discountCodes.OrderByDescending(d => d.ValidFrom).ToList());
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public ActionResult CreateDiscountCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CreateDiscountCode(CreateDiscountCodeViewModel discountCode)
        {
            if (ModelState.IsValid)
            {
                var result = await jsfService.CreateDiscountCodeAsync(discountCode);

                if (result.Success)
                    return RedirectToAction("DiscountCodes", "Admin");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateDiscountCode(long discountCodeId)
        {
            var discountCode = await jsfService.GetDiscountCodeAsync(discountCodeId);

            return View(discountCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateDiscountCode(DiscountCodeViewModel discountCode)
        {
            if (ModelState.IsValid)
            {
                var result = await jsfService.UpdateDiscountCodeAsync(discountCode);

                if (result.Success)
                    return RedirectToAction("DiscountCodes", "Admin");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CustomerPlans()
        {
            var orders = await jsfService.GetOrdersAsync();

            return View(orders);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CustomerPlan(long orderId)
        {
            var order = await jsfService.GetOrderAsync(orderId);

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UploadImage(HttpPostedFileBase[] postedFile)
        {
            if (postedFile != null && postedFile.Where(p => p != null).Any())
            {
                foreach (var file in postedFile)
                {
                    var uploadResult = fileHelper.UploadFile(file, Settings.Default.ImageDirectory);
                    if (!uploadResult.Success)
                        return RedirectToAction("ImageConfiguration", "Admin", new { errorMessage = string.Format(Resources.FailedToUploadFileErrorMessage, file.FileName) });

                    var result = await jsfService.AddImageAsync(uploadResult.UploadPath);
                    if (!result.Success)
                        return RedirectToAction("ImageConfiguration", "Admin", new { errorMessage = string.Format(Resources.FailedToAddImageToDatabaseErrorMessage, uploadResult.UploadPath) });
                }
            }

            return RedirectToAction("ImageConfiguration", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DeleteImage(long imageId)
        {
            var result = await jsfService.DeleteImageAsync(imageId);
            if (!result)
                return RedirectToAction("ImageConfiguration", "Admin", new { errorMessage = string.Format(Resources.FailedToDeleteImageErrorMessage, imageId) });

            return RedirectToAction("ImageConfiguration", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> ImageConfiguration(string errorMessage = null)
        {
            var imageConfiguration = await jsfService.GetImageConfigurationAsync();

            if (!string.IsNullOrEmpty(errorMessage))
                ViewBag.ErrorMessage = errorMessage;

            return View(imageConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> ImageConfiguration(ImageConfigurationViewModel imageConfiguration)
        {
            if (ModelState.IsValid)
            {
                var result = await jsfService.CreateOrUpdateImageConfigurationAsync(imageConfiguration);

                if (!result.Success)
                    return RedirectToAction("ImageConfiguration", "Admin", new { errorMessage = "An error occured configuring images." });

                return RedirectToAction("ImageConfiguration", "Admin");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UploadPlan(UploadPlanViewModel uploadPlanViewModel)
        {
            if (!ModelState.IsValid)
                return View(uploadPlanViewModel);

            var customer = await jsfService.GetCustomerDetailsAsync(uploadPlanViewModel.CustomerId);
            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resources.FailedToFindCustomerErrorMessage, uploadPlanViewModel.CustomerId));
                return View(uploadPlanViewModel);
            }

            string fileName = string.Format(Resources.PlanFilenameFormat, customer.Firstname, customer.Surname, uploadPlanViewModel.Name, uploadPlanViewModel.Description, uploadPlanViewModel.TransactionId, DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

            var uploadResult = UploadFile(uploadPlanViewModel.PostedFile, Settings.Default.PlanDirectory, fileName);
            if (!uploadResult.Success)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resources.FailedToUploadPlanForCustomerErrorMessage, uploadPlanViewModel.TransactionId, uploadPlanViewModel.CustomerId));
                return View(uploadPlanViewModel);
            }

            if (!await jsfService.UploadCustomerPlanAsync(uploadPlanViewModel.PlanId, uploadResult.UploadPath))
            {
                ModelState.AddModelError(string.Empty, string.Format(Resources.FailedToAssociatePlanToPurchaseErrorMessage, uploadResult.UploadPath, uploadPlanViewModel.TransactionId, uploadPlanViewModel.CustomerId));
                return View(uploadPlanViewModel);
            }

            var orderViewModel = await jsfService.GetOrderAsync(uploadPlanViewModel.OrderId);
            if (orderViewModel == null)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resources.OrderNotFoundErrorMessage, uploadPlanViewModel.OrderId));
                return View(uploadPlanViewModel);
            }

            if (orderViewModel.Plans != null && orderViewModel.Plans.Any() && !orderViewModel.Plans.Any(i => i.RequiresAction))
            {
                var orderCompleteResult = await jsfService.MarkOrderCompleteAsync(uploadPlanViewModel.OrderId);

                var planPaths = orderViewModel.Plans.Select(i => fileHelper.MapPath(i.PlanPath)).ToList();

                // send confirmation email
                if (!await SendOrderCompleteEmail(orderViewModel, planPaths))
                {
                    ModelState.AddModelError(string.Empty, string.Format(Resources.FailedToSendOrderCompleteEmailErrorMessage, uploadPlanViewModel.OrderId, uploadPlanViewModel.CustomerId));
                    return View(uploadPlanViewModel);
                }
            }

            return RedirectToAction("CustomerPlan", "Admin", new { orderId = uploadPlanViewModel.OrderId });
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> HallOfFame()
        {
            var hallOfFameEntries = await jsfService.GetHallOfFameEntriesAsync(false);

            return View(hallOfFameEntries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateHallOfFameStatus(long planId, bool status)
        {
            var result = await jsfService.UpdateHallOfFameStatusAsync(planId, status);

            if (result)
                return RedirectToAction("HallOfFame", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DeleteHallOfFameStatus(long planId)
        {
            var result = await jsfService.DeleteHallOfFameEntryAsync(planId);

            if (result)
                return RedirectToAction("HallOfFame", "Admin");

            return View();
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> Messages()
        {
            var messages = await jsfService.GetMessagesAsync();

            return View(messages);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> Message(long messageId)
        {
            var message = await jsfService.GetMessageAsync(messageId);

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> Message(MessageViewModel messageViewModel)
        {
            var messageResult = await jsfService.UpdateMessageAsync(messageViewModel);
            if (!messageResult.Success)
            {
                ModelState.AddModelError(string.Empty, "Failed to add response to message to database");
                return View(messageViewModel);
            }

            if (!await SendMessageResponseEmail(messageViewModel))
            {
                ModelState.AddModelError(string.Empty, $"Failed to send response message to {messageViewModel.EmailAddress}.");
                return View(messageViewModel);
            }

            return RedirectToAction("Messages", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DeleteMessage(long messageId)
        {
            var success = await jsfService.DeleteMessageAsync(messageId);

            if (!success)
                logger.Warn($"Failed to delete message with id {messageId}.");

            return RedirectToAction("Messages", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<FileResult> DownloadMailingList()
        {
            var mailingList = await jsfService.GetMailingListAsync();

            string fileName = string.Format(Resources.MailingListFilename, DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            return File(Encoding.UTF8.GetBytes(string.Join("\r\n", mailingList.OrderBy(s => s.Email).Select(s => s.Email).ToList())), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private UploadResult UploadFile(HttpPostedFileBase postedFile, string directory, string name = null)
        {
            var uploadResult = fileHelper.UploadFile(postedFile, directory, name);
            if (!uploadResult.Success)
            {
                ModelState.AddModelError(string.Empty, string.Format(Resources.FailedToUploadFileErrorMessage, postedFile.FileName));
            }

            return uploadResult;
        }

        private async Task<bool> SendOrderCompleteEmail(OrderHistoryViewModel orderViewModel, IEnumerable<string> planPaths)
        {
            var email = this.RenderRazorViewToString("_OrderComplete", orderViewModel, RootUri);

            return await jsfService.SendEmailAsync(string.Format(Resources.OrderComplete, orderViewModel.TransactionId), email, new List<string>() { orderViewModel.Customer.EmailAddress }, planPaths);
        }

        private async Task<bool> SendMessageResponseEmail(MessageViewModel messageViewModel)
        {
            var email = this.RenderRazorViewToString("_EmailMessageResponse", messageViewModel, RootUri);

            return await jsfService.SendEmailAsync(string.Format(Resources.MessageResponseSubject, messageViewModel.Subject), email, new List<string>() { messageViewModel.EmailAddress });
        }
    }
}