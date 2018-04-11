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
using System.Collections.Specialized;
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
        public class CompletePayment
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            PaymentResult paymentResult;
            string payerId = "payerId";
            string paymentId = "paymentId";
            string transactionId = "transactionId";
            long orderId = 1234;
            string requestUrl = "requesturl";
            string requestScheme = "https";
            string emailAddress = "emailAddress";
            OrderHistoryViewModel purchaseHistoryViewModel;
            ConfirmOrderViewModel confirmOrderViewModel;
            RouteData routeData = new RouteData();
            Mock<IView> viewMock;
            Mock<IViewEngine> engineMock;
            ViewEngineResult viewEngineResultMock;
            IEnumerable<string> emailAddressesCallback;
            string emailSubjectCallback;

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
                routeData.Values.Add("HomeController", "_OrderConfirmation");
                contextMock.SetupGet(m => m.RouteData).Returns(routeData);

                viewMock = new Mock<IView>();
                engineMock = new Mock<IViewEngine>();
                viewEngineResultMock = new ViewEngineResult(viewMock.Object, engineMock.Object);
                engineMock.Setup(e => e.FindPartialView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(viewEngineResultMock);
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(engineMock.Object);

                purchaseHistoryViewModel = new OrderHistoryViewModel()
                {
                    TransactionId = transactionId,
                    Customer = new CustomerViewModel()
                    {
                        EmailAddress = emailAddress
                    },
                };

                confirmOrderViewModel = new ConfirmOrderViewModel()
                {
                    CustomerDetails = new CustomerViewModel()
                    {
                        EmailAddress = emailAddress,
                    }
                };

                paymentResult = new PaymentResult()
                {
                    Success = true,
                };

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                requestMock.SetupGet(r => r.Params)
                           .Returns(new NameValueCollection() { { SessionKeys.PayerId, payerId } });
                sessionMock[SessionKeys.PayerId] = payerId;
                sessionMock[SessionKeys.PaymentId] = paymentId;
                sessionMock[SessionKeys.TransactionId] = transactionId;
                sessionMock[SessionKeys.ConfirmOrderViewModel] = confirmOrderViewModel;

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                jsfServiceMock.Setup(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(paymentResult);
                jsfServiceMock.Setup(s => s.GetOrderAsync(It.IsAny<long>()))
                              .ReturnsAsync(purchaseHistoryViewModel);
                emailAddressesCallback = new List<string>();
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                              .Callback<string, string, IEnumerable<string>>((a, b, c) => { emailSubjectCallback = a; emailAddressesCallback = c; })
                              .ReturnsAsync(true);
                jsfServiceMock.Setup(s => s.SaveOrderAsync(It.IsAny<ConfirmOrderViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = orderId });

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void CompletePayment_PayerIdNull_ReturnsRedirectToRouteResult()
            {
                // setup
                sessionMock.Remove(SessionKeys.PayerId);

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.PaymentCompletionErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void CompletePayment_PaymentIdNull_ReturnsRedirectToRouteResult()
            {
                // setup
                sessionMock.Remove(SessionKeys.PaymentId);

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.PaymentCompletionErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void CompletePayment_TransactionIdNull_ReturnsRedirectToRouteResult()
            {
                // setup
                sessionMock.Remove(SessionKeys.TransactionId);

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.PaymentCompletionErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void CompletePayment_CompletePaymentFails_ReturnsRedirectToRouteResult()
            {
                // setup
                paymentResult.Success = false;
                paymentResult.ErrorMessage = "Failed to complete payment";

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.FailedToCompletePayPalPaymentErrorMessage, paymentResult.ErrorMessage), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void CompletePayment_GetPurchaseAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetOrderAsync(It.IsAny<long>()))
                              .ReturnsAsync((OrderHistoryViewModel)null);

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.FailedToRetrieveOrderErrorMessage, orderId, transactionId), result.RouteValues["errorMessage"]);

                // verify the session variables have been added
                Assert.AreEqual(1, sessionMock.Count);
                Assert.IsFalse((bool)sessionMock[SessionKeys.HallOfFame]);
            }

            [TestMethod]
            public void CompletePayment_SendOrderConfirmationEmailFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                              .Callback<string, string, IEnumerable<string>>((a, b, c) => { emailSubjectCallback = a; emailAddressesCallback = c; })
                              .ReturnsAsync(false);

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("PaymentConfirmation", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(transactionId, result.RouteValues["transactionId"]);

                // verify the session variables have been added
                Assert.AreEqual(1, sessionMock.Count);
                Assert.IsFalse((bool)sessionMock[SessionKeys.HallOfFame]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallback);
                Assert.AreEqual(1, emailAddressesCallback.Count());
                Assert.AreEqual(emailAddress, emailAddressesCallback.First());
                Assert.IsNotNull(emailSubjectCallback);
                Assert.AreEqual(string.Format(Settings.Default.OrderConfirmation, transactionId), emailSubjectCallback);
            }

            [TestMethod]
            public void CompletePayment_Success_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SaveOrderAsync(It.IsAny<ConfirmOrderViewModel>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("PaymentConfirmation", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(transactionId, result.RouteValues["transactionId"]);

                // verify the session variables have been added
                Assert.AreEqual(1, sessionMock.Count);
                Assert.IsFalse((bool)sessionMock[SessionKeys.HallOfFame]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallback);
                Assert.AreEqual(1, emailAddressesCallback.Count());
                Assert.AreEqual(emailAddress, emailAddressesCallback.First());
                Assert.IsNotNull(emailSubjectCallback);
                Assert.AreEqual(string.Format(Settings.Default.OrderConfirmation, transactionId), emailSubjectCallback);
            }

            [TestMethod]
            public void CompletePayment_HallOfFameVisible_ReturnsRedirectToRouteResult()
            {
                // setup
                sessionMock[SessionKeys.HallOfFame] = true;

                // test
                var result = controller.CompletePayment().Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.CompletePayPalPayment(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetOrderAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("PaymentConfirmation", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(transactionId, result.RouteValues["transactionId"]);

                // verify the session variables have been added
                Assert.AreEqual(1, sessionMock.Count);
                Assert.IsTrue((bool)sessionMock[SessionKeys.HallOfFame]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallback);
                Assert.AreEqual(1, emailAddressesCallback.Count());
                Assert.AreEqual(emailAddress, emailAddressesCallback.First());
                Assert.IsNotNull(emailSubjectCallback);
                Assert.AreEqual(string.Format(Settings.Default.OrderConfirmation, transactionId), emailSubjectCallback);
            }

        }
    }
}
