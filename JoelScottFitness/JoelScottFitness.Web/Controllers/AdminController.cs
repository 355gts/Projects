using JoelScottFitness.Common.Constants;
using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Helpers;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Enumerations;
using JoelScottFitness.Identity.Models;
using JoelScottFitness.Services.Services;
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
        public async Task<ActionResult> Blogs()
        {
            var blogs = await jsfService.GetBlogsAsync();

            return View(blogs.OrderByDescending(b => b.CreatedDate));
        }

        [HttpGet]
        public ActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBlog(CreateBlogViewModel blog, HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
                ModelState.AddModelError("NoImage", "You must select an image");

            if (ModelState.IsValid)
            {
                blog.ImagePath = UploadImage(postedFile, "Content/Images");

                var result = await jsfService.CreateBlogAsync(blog);

                if (result.Success)
                    return RedirectToAction("Blogs", "Admin");
            }

            return View(blog);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateBlog(long blogId)
        {
            var blog = await jsfService.GetBlogAsync(blogId);

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBlog(BlogViewModel blog, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    blog.ImagePath = UploadImage(postedFile, "Content/Images");
                }

                var result = await jsfService.UpdateBlogAsync(blog);

                if (result.Success)
                    return RedirectToAction("Blogs", "Admin");
            }

            return View(blog);
        }

        [HttpGet]
        public async Task<ActionResult> Plans()
        {
            var plans = await jsfService.GetPlansAsync();

            return View(plans.OrderByDescending(p => p.CreatedDate));
        }

        [HttpGet]
        public ActionResult CreatePlan()
        {
            return View(new CreatePlanViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePlan(CreatePlanViewModel plan, HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
                ModelState.AddModelError("NoImage", "You must select an image");

            if (ModelState.IsValid)
            {
                plan.ImagePathLarge = UploadImage(postedFile, "Content/Images");

                var result = await jsfService.CreatePlanAsync(plan);

                if (result.Success)
                    return RedirectToAction("Plans", "Admin");
            }

            return View(plan);
        }

        [HttpGet]
        public async Task<ActionResult> UpdatePlan(long planId)
        {
            var plan = await jsfService.GetPlanAsync(planId);

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePlan(PlanViewModel plan, HttpPostedFileBase postedFile)
        {
            if (ModelState.IsValid)
            {
                if (postedFile != null)
                {
                    plan.ImagePathLarge = UploadImage(postedFile, "Content/Images");
                }

                var result = await jsfService.UpdatePlanAsync(plan);

                if (result.Success)
                    return RedirectToAction("Plans", "Admin");
            }

            return View(plan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBlogStatus(long blogId, bool status)
        {
            var result = await jsfService.UpdateBlogStatusAsync(blogId, status);

            if (result)
                return RedirectToAction("Blogs", "Admin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePlanStatus(long planId, bool status)
        {
            var result = await jsfService.UpdatePlanStatusAsync(planId, status);

            if (result)
                return RedirectToAction("Plans", "Admin");

            return View();
        }

        private string UploadImage(HttpPostedFileBase postedFile, string directory)
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
            catch(Exception ex)
            {
                // TODO log exception
            }

            return uploadPath;
        }
    }
}