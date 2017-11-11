using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Helpers;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JoelScottFitness.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IJSFitnessService jsfService;
        private readonly IHelper helper;

        public AdminController(IJSFitnessService jsfService,
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
                blog.ImagePath = SaveImage(postedFile, "Content/Images");

                if (blog.BlogImages != null && blog.BlogImages.Any())
                {
                    foreach (var blogImage in blog.BlogImages)
                    {
                        blogImage.ImagePath = SaveImage(blogImage.PostedFile, "Content/Images");
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
                    blog.ImagePath = SaveImage(postedFile, "Content/Images");
                }

                if (blog.BlogImages != null && blog.BlogImages.Any())
                {
                    foreach (var blogImage in blog.BlogImages.Where(i => i.PostedFile != null).ToList())
                    {
                        blogImage.ImagePath = SaveImage(blogImage.PostedFile, "Content/Images");
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
                plan.ImagePathLarge = SaveImage(postedFile, "Content/Images");

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
                    plan.ImagePathLarge = SaveImage(postedFile, "Content/Images");
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
        public async Task<ActionResult> UploadImage(HttpPostedFileBase[] postedFile)
        {
            var postedFiles = postedFile.ToList();

            foreach (var file in postedFile)
            {
                string imagePath = SaveImage(file, "Content/Images");

                var result = await jsfService.AddImage(imagePath);

                if (!result.Success)
                {
                    // TODO log error
                }

            }
            
            return RedirectToAction("ImageConfiguration", "Admin");
        }

        [HttpGet]
        public async Task<ActionResult> ImageConfiguration(string imageConfigurationError = null)
        {
            var imageConfiguration = await jsfService.GetImageConfiguration();
            
            return View(imageConfiguration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        private string SaveImage(HttpPostedFileBase postedFile, string directory)
        {
            string uploadPath = null;
            try
            {
                string path = Server.MapPath($"~/{directory}/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                uploadPath = $"/{directory}/{Path.GetFileName(postedFile.FileName)}";
            }
            catch (Exception ex)
            {
                // TODO log exception
            }

            return uploadPath;
        }


    }
}