using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Common.Results;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Helpers;
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
        public class GuestDetails
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;

            Guid customerId = Guid.NewGuid();
            string emailAddress = "EmailAddress";
            CreateCustomerViewModel customerViewModel;

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


                customerViewModel = new CreateCustomerViewModel()
                {
                    EmailAddress = emailAddress,
                    JoinMailingList = true,
                };

                jsfServiceMock.Setup(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()))
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
            public void GuestDetails_ModelStateInvalid_ReturnsView()
            {
                // setup
                controller.ModelState.AddModelError(string.Empty, "Error");

                // test
                var result = controller.GuestDetails(customerViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()), Times.Never);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GuestDetails_CreateCustomerAsyncFails_AddsModelError_ReturnsView()
            {
                // setup
                jsfServiceMock.Setup(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()))
                              .ReturnsAsync(new AsyncResult<Guid>() { Success = false, Result = customerId });

                // test
                var result = controller.GuestDetails(customerViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);

                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual("An error occured saving customer details please try again.", controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void GuestDetails_JoinMailingListFalse_ReturnsRedirectToRouteResult()
            {
                // setup
                customerViewModel.JoinMailingList = false;

                // test
                var result = controller.GuestDetails(customerViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(customerId, result.RouteValues["customerId"]);
                Assert.AreEqual("ConfirmPurchase", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
            }

            [TestMethod]
            public void GuestDetails_JoinMailingListTrue_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.GuestDetails(customerViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CreateCustomerAsync(It.IsAny<CreateCustomerViewModel>(), It.IsAny<long?>()), Times.Once);
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(customerId, result.RouteValues["customerId"]);
                Assert.AreEqual("ConfirmPurchase", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
            }
        }
    }
}
