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
using System.Web;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class CheckoutWithPaypal
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            Guid customerId = Guid.NewGuid();
            string paymentId = "paymentId";
            string transactionId = "transactionId";
            string requestUrl = "requesturl";
            string requestScheme = "https";

            BasketViewModel basket;
            CustomerViewModel customerViewModel;
            PaymentInitiationResult paymentInitiationResult;

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

                basket = new BasketViewModel();
                customerViewModel = new CustomerViewModel();
                paymentInitiationResult = new PaymentInitiationResult()
                {
                    PaymentId = paymentId,
                    TransactionId = transactionId,
                    Success = true,
                    PayPalRedirectUrl = "PayPalRedirectUrl",
                    ErrorMessage = "ErrorMessage",
                };

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                basketHelperMock.Setup(b => b.GetBasket())
                                .Returns(basket);

                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(customerViewModel);
                jsfServiceMock.Setup(s => s.InitiatePayPalPayment(It.IsAny<ConfirmOrderViewModel>(), It.IsAny<string>()))
                              .Returns(paymentInitiationResult);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void CheckoutWithPaypal_CustomerIdNull_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.CheckoutWithPaypal(Guid.Empty).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Never);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmOrderViewModel>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.CustomerIdNullErrorMessage, result.RouteValues["errorMessage"]);

                Assert.AreEqual(0, sessionMock.Count);
            }

            [TestMethod]
            public void CheckoutWithPaypal_GetBasketReturnsNull_ReturnsRedirectToRouteResult()
            {
                // steup
                basketHelperMock.Setup(b => b.GetBasket())
                                .Returns((BasketViewModel)null);

                // test
                var result = controller.CheckoutWithPaypal(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmOrderViewModel>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.BasketItemsNullErrorMessage, customerId), result.RouteValues["errorMessage"]);

                Assert.AreEqual(0, sessionMock.Count);
            }

            [TestMethod]
            public void CheckoutWithPaypal_GetCustomerDetailsAsyncReturnsNull_ReturnsRedirectToRouteResult()
            {
                // steup
                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((CustomerViewModel)null);

                // test
                var result = controller.CheckoutWithPaypal(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmOrderViewModel>(), It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.GetCustomerDetailsAsyncErrorMessage, customerId), result.RouteValues["errorMessage"]);

                Assert.AreEqual(0, sessionMock.Count);
            }

            [TestMethod]
            public void CheckoutWithPaypal_InitiatePayPalPaymentFails_ReturnsRedirectToRouteResult()
            {
                // steup
                paymentInitiationResult.Success = false;

                // test
                var result = controller.CheckoutWithPaypal(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmOrderViewModel>(), It.IsAny<string>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.FailedToInitiatePayPalPaymentErrorMessage, paymentInitiationResult.ErrorMessage), result.RouteValues["errorMessage"]);

                Assert.AreEqual(0, sessionMock.Count);
            }

            [TestMethod]
            public void CheckoutWithPaypal_Success_ReturnsRedirectResult()
            {
                // test
                var result = controller.CheckoutWithPaypal(customerId).Result as RedirectResult;

                // verify
                Assert.IsNotNull(result);
                Assert.IsNotNull(paymentInitiationResult.PayPalRedirectUrl, result.Url);

                Assert.AreEqual(3, sessionMock.Count);
                Assert.AreEqual(paymentId, (string)sessionMock[SessionKeys.PaymentId]);
                Assert.AreEqual(transactionId, (string)sessionMock[SessionKeys.TransactionId]);
                Assert.IsNotNull((ConfirmOrderViewModel)sessionMock[SessionKeys.ConfirmOrderViewModel]);

                var confirmOrderViewModel = (ConfirmOrderViewModel)sessionMock[SessionKeys.ConfirmOrderViewModel];
                Assert.AreEqual(paymentInitiationResult.PaymentId, confirmOrderViewModel.PayPalReference);
                Assert.AreEqual(paymentInitiationResult.TransactionId, confirmOrderViewModel.TransactionId);
            }
        }
    }
}
