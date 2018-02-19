using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Constants;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public class RemoveDiscountCode
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

                sessionMock[SessionKeys.DiscountCode] = discountCode;
                
                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void RemoveDiscountCode_DiscountNotInSession_DoesNotRemove_ReturnsJsonResultAppliedFalse()
            {
                // setup
                sessionMock.Remove(SessionKeys.DiscountCode);

                // test
                var result = controller.RemoveDiscountCode() as JsonResult;

                // verify
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["applied"]);

                // verify the discount code has NOT been added to the session
                Assert.AreEqual(0, sessionMock.Keys.Count);
            }

            [TestMethod]
            public void RemoveDiscountCode_RemovesDiscountCodeFromSession_ReturnsJsonResultAppliedFalse()
            {
                // test
                var result = controller.RemoveDiscountCode() as JsonResult;

                // verify
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Data);

                IDictionary<string, object> data = new RouteValueDictionary(result.Data);

                // verify the json result contains the correct properties
                Assert.IsFalse((bool)data["applied"]);

                // verify the discount code has NOT been added to the session
                Assert.AreEqual(0, sessionMock.Keys.Count);
            }
        }
    }
}
