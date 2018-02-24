using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class MyPlans
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            Mock<IPrincipal> user;
            Mock<IIdentity> identity;
            string userId = "userId";
            Guid id = Guid.NewGuid();
            CustomerViewModel customerViewModel;

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

                user = new Mock<IPrincipal>();
                identity = new Mock<IIdentity>();
                user.Setup(u => u.Identity).Returns(identity.Object);
                identity.Setup(i => i.Name).Returns(userId);

                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);
                contextMock.Setup(c => c.HttpContext.User)
                           .Returns(user.Object);

                customerViewModel = new CustomerViewModel()
                {
                    Id = id,
                    EmailAddress = userId,
                };

                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<string>()))
                              .ReturnsAsync(customerViewModel);
                jsfServiceMock.Setup(s => s.GetCustomerPlansAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(new List<PurchasedHistoryItemViewModel>() { new PurchasedHistoryItemViewModel(), new PurchasedHistoryItemViewModel() });

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void MyPlans_GetCustomerDetailsAsync_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<string>()))
                              .ReturnsAsync((CustomerViewModel)null);

                // test
                var result = controller.MyPlans().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerPlansAsync(It.IsAny<Guid>()), Times.Never);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.FailedToFindUserErrorMessage, userId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void MyPlans_GetPurchaseSummaryAsync_ReturnsNull()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetCustomerPlansAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((IEnumerable<PurchasedHistoryItemViewModel>)null);

                // test
                var result = controller.MyPlans().Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerPlansAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.IsNull(result.Model);
            }

            [TestMethod]
            public void MyPlans_Success_ReturnsPurchases()
            {
                // test
                var result = controller.MyPlans().Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerPlansAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                var purchaseHistoryItemsViewModel = (IEnumerable<PurchasedHistoryItemViewModel>)result.Model;
                Assert.AreEqual(2, purchaseHistoryItemsViewModel.Count());
            }
        }
    }
}
