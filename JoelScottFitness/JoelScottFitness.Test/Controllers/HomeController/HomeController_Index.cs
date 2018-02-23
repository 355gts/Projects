using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Constants;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using JoelScottFitness.YouTube.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class IndexTests
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;

            IList<BlogViewModel> blogs;

            string videoId1 = "111";
            string videoId2 = "222";
            string videoDescription1 = "videoDescription1";
            string videoDescription2 = "videoDescription2";
            YouTubeVideo video1;
            YouTubeVideo video2;
            IList<YouTubeVideo> videos;

            string sectionImage1 = "sectionImage1";
            string sectionImage2 = "sectionImage2";
            string sectionImage3 = "sectionImage3";
            string splashImage = "splashImage";
            SectionImageViewModel sectionImages;

            string kaleidoscopeImage1 = "kaleidoscopeImage1";
            string kaleidoscopeImage2 = "kaleidoscopeImage2";
            string kaleidoscopeImage3 = "kaleidoscopeImage3";
            string kaleidoscopeImage4 = "kaleidoscopeImage4";
            string kaleidoscopeImage5 = "kaleidoscopeImage5";
            KaleidoscopeViewModel kaleidoscopeViewModel;

            string hallOfFameName = "hallOfFameName";
            HallOfFameViewModel hallOfFameViewModel;
            IList<HallOfFameViewModel> hallOfFameViewModels;

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
                
                blogs = new List<BlogViewModel>()
                {
                    new BlogViewModel(),
                    new BlogViewModel(),
                };

                video1 = new YouTubeVideo() { VideoId = videoId1, Description = videoDescription1 };
                video2 = new YouTubeVideo() { VideoId = videoId2, Description = videoDescription2 };
                videos = new List<YouTubeVideo>()
                {
                    video1,
                    video2,
                };

                sectionImages = new SectionImageViewModel()
                {
                    SectionImage1 = sectionImage1,
                    SectionImage2 = sectionImage2,
                    SectionImage3 = sectionImage3,
                    SplashImage = splashImage,
                };

                kaleidoscopeViewModel = new KaleidoscopeViewModel()
                {
                    Image1 = kaleidoscopeImage1,
                    Image2 = kaleidoscopeImage2,
                    Image3 = kaleidoscopeImage3,
                    Image4 = kaleidoscopeImage4,
                    Image5 = kaleidoscopeImage5,
                };

                hallOfFameViewModel = new HallOfFameViewModel()
                {
                    Name = hallOfFameName,
                };
                hallOfFameViewModels = new List<HallOfFameViewModel>()
                {
                    hallOfFameViewModel
                };

                jsfServiceMock.Setup(s => s.GetBlogsAsync(It.IsAny<int>()))
                              .ReturnsAsync(blogs);
                jsfServiceMock.Setup(s => s.GetSectionImagesAsync())
                              .ReturnsAsync(sectionImages);
                jsfServiceMock.Setup(s => s.GetKaleidoscopeImagesAsync())
                              .ReturnsAsync(kaleidoscopeViewModel);
                jsfServiceMock.Setup(s => s.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()))
                              .ReturnsAsync(hallOfFameViewModels);

                youtubeClientMock.Setup(y => y.GetVideos(It.IsAny<long>()))
                                 .Returns(videos);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void Index_SectionImagesNull_ReturnsDefault()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetSectionImagesAsync())
                              .ReturnsAsync((SectionImageViewModel)null);

                // test
                var result = controller.Index(false).Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var indexViewModel = (IndexViewModel)result.Model;
                Assert.IsNotNull(indexViewModel);

                Assert.AreEqual(blogs.Count(), indexViewModel.Blogs.Count());
                Assert.AreEqual(videos.Count(), indexViewModel.Videos.Count());
                Assert.IsNotNull(indexViewModel.SectionImages);
                Assert.IsNotNull(indexViewModel.KaleidoscopeImages);
                Assert.IsNotNull(indexViewModel.LatestHallOfFamer);

                // check section images
                Assert.AreEqual(Settings.Default.DefaultSectionImage1, indexViewModel.SectionImages.SectionImage1);
                Assert.AreEqual(Settings.Default.DefaultSectionImage2, indexViewModel.SectionImages.SectionImage2);
                Assert.AreEqual(Settings.Default.DefaultSectionImage3, indexViewModel.SectionImages.SectionImage3);
                Assert.AreEqual(Settings.Default.DefaultSplashImage, indexViewModel.SectionImages.SplashImage);

                // check session variables
                Assert.AreEqual(1, sessionMock.Keys.Count);
                Assert.IsTrue((bool)sessionMock[SessionKeys.HallOfFame]);
            }

            [TestMethod]
            public void Index_ChristmasTrue_SetsSessionVariable()
            {
                // test
                var result = controller.Index(true).Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var indexViewModel = (IndexViewModel)result.Model;
                Assert.IsNotNull(indexViewModel);

                Assert.AreEqual(blogs.Count(), indexViewModel.Blogs.Count());
                Assert.AreEqual(videos.Count(), indexViewModel.Videos.Count());
                Assert.IsNotNull(indexViewModel.SectionImages);
                Assert.IsNotNull(indexViewModel.KaleidoscopeImages);
                Assert.IsNotNull(indexViewModel.LatestHallOfFamer);

                // check section images
                Assert.AreEqual(sectionImage1, indexViewModel.SectionImages.SectionImage1);
                Assert.AreEqual(sectionImage2, indexViewModel.SectionImages.SectionImage2);
                Assert.AreEqual(sectionImage3, indexViewModel.SectionImages.SectionImage3);
                Assert.AreEqual(splashImage, indexViewModel.SectionImages.SplashImage);

                // check session variables
                Assert.AreEqual(2, sessionMock.Keys.Count);
                Assert.IsTrue((bool)sessionMock[SessionKeys.HallOfFame]);
                Assert.IsTrue((bool)sessionMock[SessionKeys.Christmas]);
            }

            [TestMethod]
            public void Index_LatestHallOfFamerNull_HallOfFameSessionFalse()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetHallOfFameEntriesAsync(It.IsAny<bool>(), It.IsAny<int?>()))
                              .ReturnsAsync(Enumerable.Empty<HallOfFameViewModel>);

                // test
                var result = controller.Index(false).Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var indexViewModel = (IndexViewModel)result.Model;
                Assert.IsNotNull(indexViewModel);

                Assert.AreEqual(blogs.Count(), indexViewModel.Blogs.Count());
                Assert.AreEqual(videos.Count(), indexViewModel.Videos.Count());
                Assert.IsNotNull(indexViewModel.SectionImages);
                Assert.IsNotNull(indexViewModel.KaleidoscopeImages);
                Assert.IsNull(indexViewModel.LatestHallOfFamer);

                // check session variables
                Assert.AreEqual(1, sessionMock.Keys.Count);
                Assert.IsFalse((bool)sessionMock[SessionKeys.HallOfFame]);
            }

            [TestMethod]
            public void Index_Success()
            {
                // test
                var result = controller.Index(false).Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var indexViewModel = (IndexViewModel)result.Model;
                Assert.IsNotNull(indexViewModel);

                Assert.AreEqual(blogs.Count(), indexViewModel.Blogs.Count());
                Assert.AreEqual(videos.Count(), indexViewModel.Videos.Count());
                Assert.IsNotNull(indexViewModel.SectionImages);
                Assert.IsNotNull(indexViewModel.KaleidoscopeImages);
                Assert.IsNotNull(indexViewModel.LatestHallOfFamer);

                // check section images
                Assert.AreEqual(sectionImage1, indexViewModel.SectionImages.SectionImage1);
                Assert.AreEqual(sectionImage2, indexViewModel.SectionImages.SectionImage2);
                Assert.AreEqual(sectionImage3, indexViewModel.SectionImages.SectionImage3);
                Assert.AreEqual(splashImage, indexViewModel.SectionImages.SplashImage);
                
                // check session variables
                Assert.AreEqual(1, sessionMock.Keys.Count);
                Assert.IsTrue((bool)sessionMock[SessionKeys.HallOfFame]);
            }

        }
    }
}
