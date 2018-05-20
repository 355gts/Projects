using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Extensions;
using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class Blogs
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;

            long blogId1 = 111;
            string blogTitle1 = "blogTitle1";
            string blogTitle2 = "blogTitle2";
            string blogTitle3 = "blogTitle3";
            DateTime blogDate1 = DateTime.UtcNow.AddDays(-1);
            DateTime blogDate2 = DateTime.UtcNow;
            DateTime blogDate3 = DateTime.UtcNow.AddDays(1);
            BlogViewModel blog1;
            BlogViewModel blog2;
            BlogViewModel blog3;
            IList<BlogViewModel> blogs;

            CON.HomeController controller;

            [TestInitialize]
            public void TestSetup()
            {
                jsfServiceMock = new Mock<IJSFitnessService>();
                youtubeClientMock = new Mock<IYouTubeClient>();
                basketHelperMock = new Mock<IBasketHelper>();
                fileHelperMock = new Mock<IFileHelper>();
                contextMock = new Mock<ControllerContext>();
                sessionMock = new MockHttpSessionBase();

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);

                blog1 = new BlogViewModel()
                {
                    Title = blogTitle1,
                    CreatedDate = blogDate1,
                    Active = true,
                    SubHeader = "SubHeader",
                    Content = "Content",
                    BlogImages = new List<BlogImageViewModel>()
                    {
                        new BlogImageViewModel()
                        {
                            BlogId =123,
                            CaptionTitle = "CaptionTitle",
                            Caption = "Caption",
                            CaptionColour = BlogCaptionTextColour.White,
                            ImagePath = "ImagePath",
                            Id = 456,
                        }
                    }
                };
                blog2 = new BlogViewModel() { Title = blogTitle2, CreatedDate = blogDate2, Active = true };
                blog3 = new BlogViewModel() { Title = blogTitle3, CreatedDate = blogDate3 };
                blogs = new List<BlogViewModel>()
                {
                    blog1,
                    blog2,
                    blog3
                };
                
                jsfServiceMock.Setup(s => s.GetBlogsAsync(It.IsAny<int>()))
                              .ReturnsAsync(blogs);
                jsfServiceMock.Setup(s => s.GetBlogAsync(It.IsAny<long>()))
                              .ReturnsAsync(blog1);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void GetBlogs_GetBlogsAsyncReturnsNull_ReturnsNullModel()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetBlogsAsync(It.IsAny<int>()))
                              .ReturnsAsync(Enumerable.Empty<BlogViewModel>);

                // test
                var result = controller.Blogs().Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var blogsViewModel = (IOrderedEnumerable<BlogViewModel>)result.Model;
                Assert.IsNull(blogsViewModel);
            }

            [TestMethod]
            public void GetBlogs_GetBlogsAsyncNonActive_ReturnsEmptyList()
            {
                // setup
                blog1.Active = false;
                blog2.Active = false;

                // test
                var result = controller.Blogs().Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var blogsViewModel = (IOrderedEnumerable<BlogViewModel>)result.Model;
                Assert.IsNotNull(blogsViewModel);
                Assert.AreEqual(0, blogsViewModel.Count());
            }

            [TestMethod]
            public void GetBlogs_Success()
            {
                // test
                var result = controller.Blogs().Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var blogsViewModel = (IOrderedEnumerable<BlogViewModel>)result.Model;
                Assert.IsNotNull(blogsViewModel);
                Assert.AreEqual(2, blogsViewModel.Count());
                Assert.AreEqual(blogTitle2, blogsViewModel.First().Title);
                Assert.AreEqual(blogTitle1, blogsViewModel.Skip(1).First().Title);
            }

            [TestMethod]
            public void GetBlog_Success()
            {
                // test
                var result = controller.Blog(blogId1).Result as JsonResult;

                // verify
                Assert.IsNotNull(result);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                Assert.AreEqual(blog1.Title, data["title"]);
                Assert.AreEqual(blog1.CreatedDate.DateTimeDisplayStringLong(), data["date"]);
                Assert.AreEqual(blog1.SubHeader, data["subTitle"]);
                Assert.AreEqual($"<p>{blog1.Content}</p>", data["content"]);

                var blogImages = ((IEnumerable<BlogImageViewModel>)data["images"]);

                Assert.AreEqual(1, blogImages.Count());
                Assert.AreEqual(blog1.BlogImages.First().BlogId, blogImages.First().BlogId);
                Assert.AreEqual(blog1.BlogImages.First().Caption, blogImages.First().Caption);
                Assert.AreEqual(blog1.BlogImages.First().CaptionColour, blogImages.First().CaptionColour);
                Assert.AreEqual(blog1.BlogImages.First().CaptionTitle, blogImages.First().CaptionTitle);
                Assert.AreEqual(blog1.BlogImages.First().Id, blogImages.First().Id);
                Assert.AreEqual(blog1.BlogImages.First().ImagePath, blogImages.First().ImagePath);
            }
        }
    }
}
