using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.Web.Properties;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class ExistingCustomerDetails
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;

            Guid customerId = Guid.NewGuid();
            string emailAddress = "EmailAddress";
            CustomerViewModel customer;
            UserViewModel user;

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

                user = new UserViewModel()
                {
                    EmailAddress = emailAddress,
                };

                customer = new CustomerViewModel()
                {
                    EmailAddress = emailAddress,
                    JoinMailingList = true,
                };

                jsfServiceMock.Setup(s => s.GetUserAsync(It.IsAny<string>()))
                              .ReturnsAsync(user);

                jsfServiceMock.Setup(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()))
                              .ReturnsAsync(new AsyncResult<Guid>() { Success = true, Result = customerId });

                jsfServiceMock.Setup(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()))
                              .ReturnsAsync(true);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void ExistingCustomerDetails_ModelStateInvalid_ReturnsView()
            {
                // setup
                controller.ModelState.AddModelError(string.Empty, "Error");

                // test
                var result = controller.ExistingCustomerDetails(customer).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()), Times.Never);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual("Error", controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void ExistingCustomerDetails_GetUserAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetUserAsync(It.IsAny<string>()))
                              .ReturnsAsync((UserViewModel)null);

                // test
                var result = controller.ExistingCustomerDetails(customer).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetUserAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()), Times.Never);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.UnableToFindExistingCustomerErrorMessage, customer.EmailAddress), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ExistingCustomerDetails_UpdateCustomerAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()))
                              .ReturnsAsync(new AsyncResult<Guid>() { Success = false, Result = customerId });

                // test
                var result = controller.ExistingCustomerDetails(customer).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetUserAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.FailedToUpdateExistingCustomerDetailsErrorMessage, customer.EmailAddress), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ExistingCustomerDetails_JoinMailingListFalse_ReturnsRedirectToRouteResult()
            {
                // setup
                customer.JoinMailingList = false;

                // test
                var result = controller.ExistingCustomerDetails(customer).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetUserAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(customerId, result.RouteValues["customerId"]);
                Assert.AreEqual("ConfirmOrder", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
            }

            [TestMethod]
            public void ExistingCustomerDetails_JoinMailingListTrue_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.ExistingCustomerDetails(customer).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetUserAsync(It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateCustomerAsync(It.IsAny<CustomerViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(customerId, result.RouteValues["customerId"]);
                Assert.AreEqual("ConfirmOrder", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
            }
        }
    }
}
