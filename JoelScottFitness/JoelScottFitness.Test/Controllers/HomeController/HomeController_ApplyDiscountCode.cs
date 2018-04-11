using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class ApplyDiscountCode
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;

            string discountCodeCode = "discountCode";
            long discountCodeId = 111;
            int discountPercentage = 30;
            DiscountCodeViewModel discountCode;

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

                discountCode = new DiscountCodeViewModel()
                {
                    Id = discountCodeId,
                    Code = discountCodeCode,
                    PercentDiscount = discountPercentage,
                    ValidFrom = DateTime.UtcNow.AddDays(-1),
                    ValidTo = DateTime.UtcNow.AddDays(1),
                };

                jsfServiceMock.Setup(s => s.GetDiscountCodeAsync(It.IsAny<string>()))
                              .ReturnsAsync(discountCode);

                basketHelperMock.Setup(b => b.AddDiscountCode(It.IsAny<DiscountCodeViewModel>()))
                                .Returns(true);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void ApplyDiscountCode_GetDiscountCodeFails_ReturnsJsonResultAppliedFalse()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetDiscountCodeAsync(It.IsAny<string>()))
                              .ReturnsAsync((DiscountCodeViewModel)null);

                // test
                var result = controller.ApplyDiscountCode(discountCodeCode).Result as JsonResult;

                // verify
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["applied"]);

                // verify the discount code has NOT been added to the session
                Assert.AreEqual(0, sessionMock.Keys.Count);
            }

            [TestMethod]
            public void ApplyDiscountCode_DiscountCodeIsNotActive_ReturnsJsonResultAppliedFalse()
            {
                // setup
                discountCode.ValidFrom = DateTime.UtcNow.AddDays(-10);
                discountCode.ValidTo = DateTime.UtcNow.AddDays(-10);

                // test
                var result = controller.ApplyDiscountCode(discountCodeCode).Result as JsonResult;

                // verify
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);
                basketHelperMock.Verify(b => b.AddDiscountCode(It.IsAny<DiscountCodeViewModel>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["applied"]);
            }

            [TestMethod]
            public void ApplyDiscountCode_Success()
            {
                // test
                var result = controller.ApplyDiscountCode(discountCodeCode).Result as JsonResult;

                // verify
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);
                basketHelperMock.Verify(b => b.AddDiscountCode(It.IsAny<DiscountCodeViewModel>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.AreEqual(discountCode.Id, (long)data["discountCodeId"]);
                Assert.AreEqual(discountCode.PercentDiscount, (int)data["discount"]);
                Assert.AreEqual($"{discountCode.PercentDiscount}% Discount!", data["description"]);
                Assert.IsTrue((bool)data["applied"]);
            }

            [TestMethod]
            public void ApplyDiscountCode_BasketHelperAddFails_Success()
            {
                // setup
                basketHelperMock.Setup(b => b.AddDiscountCode(It.IsAny<DiscountCodeViewModel>()))
                                .Returns(false);
                // test
                var result = controller.ApplyDiscountCode(discountCodeCode).Result as JsonResult;

                // verify
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<string>()), Times.Once);
                basketHelperMock.Verify(b => b.AddDiscountCode(It.IsAny<DiscountCodeViewModel>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["applied"]);
            }
        }
    }
}
