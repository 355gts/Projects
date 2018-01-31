﻿using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Extensions;
using JoelScottFitness.Web.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IJSFitnessService jsfService;
        private readonly IFileHelper fileHelper;

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
        public ActionResult CreateBlog()
        {
            return View();
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
                plan.ImagePathLarge = uploadResult.UploadPath;

                var result = await jsfService.CreatePlanAsync(plan);

                if (result.Success)
                    return RedirectToAction("Plans", "Admin");
            }

            return View(plan);
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
                    return RedirectToAction("DiscountCodes","Admin");
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
            var purchases = await jsfService.GetPurchasesAsync();

            return View(purchases);
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> CustomerPlan(long purchaseId)
        {
            var purchase = await jsfService.GetPurchaseAsync(purchaseId);

            return View(purchase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UploadImage(HttpPostedFileBase[] postedFile)
        {
            var postedFiles = postedFile.ToList();

            foreach (var file in postedFile)
            {
                var uploadResult = fileHelper.UploadFile(file, Settings.Default.ImageDirectory);

                // TODO what when upload fails

                var result = await jsfService.AddImage(uploadResult.UploadPath);

                if (!result.Success)
                {
                    // TODO log error
                }

            }
            
            return RedirectToAction("ImageConfiguration", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> ImageConfiguration(string imageConfigurationError = null)
        {
            var imageConfiguration = await jsfService.GetImageConfiguration();
            
            return View(imageConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> ImageConfiguration(ImageConfigurationViewModel imageConfiguration)
        {
            if (ModelState.IsValid)
            {
                var result = await jsfService.CreateOrUpdateImageConfiguration(imageConfiguration);

                if (!result.Success)
                    return RedirectToAction("ImageConfiguration", "Admin", new { imageConfigurationError = "An error occured configuring images." });

                return RedirectToAction("ImageConfiguration", "Admin");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UploadPlan(UploadPlanViewModel uploadPlanViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = await jsfService.GetCustomerDetailsAsync(uploadPlanViewModel.CustomerId);

                string fileName = string.Format(Settings.Default.PlanFilenameFormat, customer.Firstname, customer.Surname, uploadPlanViewModel.Name, uploadPlanViewModel.Description, DateTime.UtcNow.ToString("yyyyMMdd"));

                var uploadResult = UploadFile(uploadPlanViewModel.PostedFile, Settings.Default.PlanDirectory, fileName);
                if (!uploadResult.Success)
                {
                    // TODO what happens when upload fails
                }                

                var result = await jsfService.AssociatePlanToPurchase(uploadPlanViewModel.PurchasedItemId, uploadResult.UploadPath);

                var purchaseViewModel = await jsfService.GetPurchaseAsync(uploadPlanViewModel.PurchaseId);

                // if none of the plans require action then they are all ready
                if (!purchaseViewModel.Items.Any(i => i.RequiresAction))
                {
                    var planPaths = purchaseViewModel.Items.Select(i => Server.MapPath(i.PlanPath)).ToList();

                    // send confirmation email
                    await SendOrderCompleteEmail(purchaseViewModel, planPaths);
                }
            }

            return RedirectToAction("CustomerPlan", "Admin", new { purchaseId = uploadPlanViewModel.PurchaseId });
        }
        
        [HttpGet]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> HallOfFame()
        {
            var hallOfFameEntries = await jsfService.GetHallOfFameEntries(false);

            return View(hallOfFameEntries);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> UpdateHallOfFameStatus(long purchasedItemId, bool status)
        {
            var result = await jsfService.UpdateHallOfFameStatusAsync(purchasedItemId, status);

            if (result)
                return RedirectToAction("HallOfFame", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = JsfRoles.Admin)]
        public async Task<ActionResult> DeleteHallOfFameStatus(long purchasedItemId)
        {
            var result = await jsfService.DeleteHallOfFameEntryAsync(purchasedItemId);

            if (result)
                return RedirectToAction("HallOfFame", "Admin");

            return View();
        }
        
        private UploadResult UploadFile(HttpPostedFileBase postedFile, string directory, string name = null)
        {
            var uploadResult = fileHelper.UploadFile(postedFile, directory, name);
            if (!uploadResult.Success)
            {
                ModelState.AddModelError(string.Empty, $"Failed to save file '{postedFile.FileName}'.");
            }
            
            return uploadResult;
        }

        //private string SaveFile(HttpPostedFileBase postedFile, string directory, string name = null)
        //{
        //    string uploadPath = null;
        //    try
        //    {
        //        string path = Server.MapPath($"~/{directory}/");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        string fileName = !string.IsNullOrEmpty(name) 
        //                            ? name 
        //                            : Path.GetFileName(postedFile.FileName);

        //        postedFile.SaveAs(path + fileName);
        //        uploadPath = $"/{directory}/{fileName}";
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO log exception
        //    }

        //    return uploadPath;
        //}

        private async Task<bool> SendOrderCompleteEmail(PurchaseHistoryViewModel purchaseViewModel, IEnumerable<string> planPaths)
        {
            var email = this.RenderRazorViewToString("_OrderComplete", purchaseViewModel);

            return await jsfService.SendEmailAsync(string.Format(Settings.Default.PurchaseComplete, purchaseViewModel.TransactionId), email, new List<string>() { purchaseViewModel.Customer.EmailAddress }, planPaths);
        }
    }
}