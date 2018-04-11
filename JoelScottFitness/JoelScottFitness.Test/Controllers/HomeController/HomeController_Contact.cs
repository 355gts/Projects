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
        public class Contact
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            string requestUrl = "requesturl";
            string requestScheme = "https";
            string emailAddress = "emailAddress";
            OrderHistoryViewModel purchaseHistoryViewModel;
            RouteData routeData = new RouteData();
            Mock<IView> viewMock;
            Mock<IViewEngine> engineMock;
            ViewEngineResult viewEngineResultMock;
            IEnumerable<string> emailAddressesCallback;
            string emailSubjectCallback;
            CreateMessageViewModel createMessageViewModel;

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
                
                routeData = new RouteData();
                routeData.Values.Add("HomeController", "_EmailMessageReceived");

                contextMock.SetupGet(m => m.RouteData)
                           .Returns(routeData);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                viewMock = new Mock<IView>();
                engineMock = new Mock<IViewEngine>();
                viewEngineResultMock = new ViewEngineResult(viewMock.Object, engineMock.Object);
                engineMock.Setup(e => e.FindPartialView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(viewEngineResultMock);
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(engineMock.Object);

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                createMessageViewModel = new CreateMessageViewModel()
                {
                    JoinMailingList = true,
                    EmailAddress = "EmailAddress",
                    Message = "Message",
                    Name = "Name",
                    Subject = "Subject",
                };

                jsfServiceMock.Setup(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()))
                              .ReturnsAsync(true);

                jsfServiceMock.Setup(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = true });

                emailAddressesCallback = new List<string>();
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                              .Callback<string, string, IEnumerable<string>>((a, b, c) => { emailSubjectCallback = a; emailAddressesCallback = c; })
                              .ReturnsAsync(true);


                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void Contact_ModelStateInvalid_ReturnsViewResult()
            {
                // setup
                controller.ModelState.AddModelError(string.Empty, "Error");

                // test
                var result = controller.Contact(createMessageViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Never);
                jsfServiceMock.Verify(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);
                
                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual("Error", controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void Contact_CreateMessageAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = false });

                // test
                var result = controller.Contact(createMessageViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.FailedToCreateMessageErrorMessage,
                                                                                            createMessageViewModel.Name,
                                                                                            createMessageViewModel.EmailAddress,
                                                                                            createMessageViewModel.Subject,
                                                                                            createMessageViewModel.Message), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void Contact_SendMessageReceivedEmailFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                              .ReturnsAsync(false);

                // test
                var result = controller.Contact(createMessageViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.FailedToSendMessageErrorMessage,
                                                                                            createMessageViewModel.Name,
                                                                                            createMessageViewModel.EmailAddress,
                                                                                            createMessageViewModel.Subject,
                                                                                            createMessageViewModel.Message), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void Contact_Success_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.Contact(createMessageViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.UpdateMailingListAsync(It.IsAny<MailingListItemViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.CreateMessageAsync(It.IsAny<CreateMessageViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallback);
                Assert.AreEqual(1, emailAddressesCallback.Count());
                Assert.AreEqual(Settings.Default.JoelScottFitnessEmaillAddress, emailAddressesCallback.First());
                Assert.IsNotNull(emailSubjectCallback);
                Assert.AreEqual("New Customer Enquiry", emailSubjectCallback);
            }
        }
    }
}
