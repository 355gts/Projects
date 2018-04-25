using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class BeforeAndAfter
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;
            Mock<HttpPostedFileBase> beforeFileMock;
            Mock<HttpPostedFileBase> afterFileMock;

            int contentLength = 1234;
            string beforeImageFilename = "beforeImageFilename";
            string afterImageFilename = "afterImageFilename";
            string beforeImageUploadPath = "beforeImageUploadPath";
            string afterImageUploadPath = "afterImageUploadPath";
            string beforeImageUploadPathCallback;
            string afterImageUploadPathCallback;
            string comment = "comment";
            long purchasedItemId = 456;
            BeforeAndAfterViewModel beforeAndAfterViewModel;
            IList<string> imageFileNames;
            IList<string> imageFileNamesCallback;

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
                requestMock = new Mock<HttpRequestBase>();

                beforeFileMock = new Mock<HttpPostedFileBase>();
                beforeFileMock.Setup(b => b.ContentLength).Returns(contentLength);
                beforeFileMock.Setup(b => b.FileName).Returns(beforeImageFilename);

                afterFileMock = new Mock<HttpPostedFileBase>();
                afterFileMock.Setup(b => b.ContentLength).Returns(contentLength);
                afterFileMock.Setup(b => b.FileName).Returns(afterImageFilename);

                beforeAndAfterViewModel = new BeforeAndAfterViewModel()
                {
                    BeforeFile = beforeFileMock.Object,
                    AfterFile = afterFileMock.Object,
                    Comment = comment,
                    OrderId = purchasedItemId,
                };

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);

                imageFileNames = new List<string>();
                fileHelperMock.SetupSequence(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(new UploadResult() { Success = true, UploadPath = beforeImageUploadPath })
                              .Returns(new UploadResult() { Success = true, UploadPath = afterImageUploadPath });

                jsfServiceMock.Setup(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Callback<long, string, string, string>((a, b, c, d) =>
                              {
                                  beforeImageUploadPathCallback = b;
                                  afterImageUploadPathCallback = c;
                              })
                              .ReturnsAsync(true);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void BeforeAndAfter_ModelStateInvalid_ReturnsJsonResult()
            {
                // setup
                controller.ModelState.AddModelError(string.Empty, "Error");

                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["success"]);
                Assert.AreEqual(Resources.GenericErrorMessage, (string)data["errorMessage"]);
            }

            [TestMethod]
            public void BeforeAndAfter_UploadBeforeFileFails_ReturnsJsonResult()
            {
                // setup
                fileHelperMock.SetupSequence(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(new UploadResult() { Success = false })
                              .Returns(new UploadResult() { Success = true, UploadPath = afterImageFilename });

                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["success"]);
                Assert.AreEqual(Resources.GenericErrorMessage, (string)data["errorMessage"]);
            }

            [TestMethod]
            public void BeforeAndAfter_UploadAfterFileFails_ReturnsJsonResult()
            {
                // setup
                fileHelperMock.SetupSequence(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(new UploadResult() { Success = true, UploadPath = beforeImageFilename })
                              .Returns(new UploadResult() { Success = false });

                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["success"]);
                Assert.AreEqual(Resources.GenericErrorMessage, (string)data["errorMessage"]);
            }

            [TestMethod]
            public void BeforeAndAfter_UploadHallOfFameAsyncFails_ReturnsJsonResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(false);

                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["success"]);
                Assert.AreEqual(Resources.GenericErrorMessage, (string)data["errorMessage"]);
            }

            [TestMethod]
            public void BeforeAndAfter_FormatsFileNames_ReturnsJsonResult()
            {
                // setup
                imageFileNamesCallback = new List<string>();
                fileHelperMock.Setup(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                            .Callback<HttpPostedFileBase, string, string>((a, b, c) => { imageFileNamesCallback.Add(c); })
                              .Returns(new UploadResult() { Success = true, UploadPath = beforeImageUploadPath });

                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsTrue((bool)data["success"]);

                Assert.IsNotNull(imageFileNamesCallback);
                Assert.AreEqual(2, imageFileNamesCallback.Count);
                Assert.AreEqual(1, imageFileNamesCallback.Count(f => f == string.Format(Resources.BeforeFileNameFormat, purchasedItemId, beforeImageFilename)));
                Assert.AreEqual(1, imageFileNamesCallback.Count(f => f == string.Format(Resources.AfterFileNameFormat, purchasedItemId, afterImageFilename)));
            }

            [TestMethod]
            public void BeforeAndAfter_Success_ReturnsJsonResult()
            {
                // test
                var result = controller.BeforeAndAfter(beforeAndAfterViewModel).Result as JsonResult;

                // verify
                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
                jsfServiceMock.Verify(s => s.UploadHallOfFameAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsTrue((bool)data["success"]);

                Assert.IsNotNull(beforeImageUploadPathCallback);
                Assert.AreEqual(beforeImageUploadPath, beforeImageUploadPathCallback);
                Assert.IsNotNull(afterImageUploadPathCallback);
                Assert.AreEqual(afterImageUploadPath, afterImageUploadPathCallback);
            }
        }
    }
}
