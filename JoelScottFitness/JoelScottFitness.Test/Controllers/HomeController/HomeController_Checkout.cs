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
using System.Web;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class Checkout
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            long discountCodeId = 555;
            int discountPercentage = 20;
            long planId1 = 111;
            long planId2 = 222;
            long planId3 = 333;
            double planPrice1 = 10.50;
            double planPrice2 = 15.00;
            PlanViewModel plan1;
            PlanViewModel plan2;
            PlanViewModel plan3;
            IList<PlanViewModel> plans;
            string emailAddress = "emailAddress";
            ConfirmPurchaseViewModel confirmPurchaseViewModel;
            ConfirmPurchaseViewModel confirmPurchaseViewModelCallback;
            SelectedPlanOptionViewModel basketItem1;
            SelectedPlanOptionViewModel basketItem2;
            IList<SelectedPlanOptionViewModel> basketItems;
            DiscountCodeViewModel discountCode;
            PaymentInitiationResult paymentInitiationResult;
            string paymentId = "paymentId";
            string transationId = "transactionId";
            string redirecturl = "requesturl";
            long purchaseId = 666;
            string callBackUriCallback;
            string requestUrl = "requesturl";
            string requestScheme = "https";

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

                basketItem1 = new SelectedPlanOptionViewModel() { PlanId = planId1, Price = planPrice1 };
                basketItem2 = new SelectedPlanOptionViewModel() { PlanId = planId2, Price = planPrice2 };
                basketItems = new List<SelectedPlanOptionViewModel>() { basketItem1, basketItem2 };

                confirmPurchaseViewModel = new ConfirmPurchaseViewModel()
                {
                    DiscountCodeId = discountCodeId,
                    BasketItems = basketItems,
                    CustomerDetails = new CustomerViewModel()
                    {
                        EmailAddress = emailAddress,
                    },
                };

                plan1 = new PlanViewModel() { Id = planId1 };
                plan2 = new PlanViewModel() { Id = planId2 };
                plan3 = new PlanViewModel() { Id = planId3 };
                plans = new List<PlanViewModel>() { plan1, plan2, plan3 };

                discountCode = new DiscountCodeViewModel() { Id = discountCodeId, PercentDiscount = discountPercentage };

                paymentInitiationResult = new PaymentInitiationResult()
                {
                    PaymentId = paymentId,
                    TransactionId = transationId,
                    PayPalRedirectUrl = redirecturl,
                    Success = true,
                };

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                jsfServiceMock.Setup(s => s.GetPlansAsync())
                              .ReturnsAsync(plans);
                jsfServiceMock.Setup(s => s.GetDiscountCodeAsync(It.IsAny<long>()))
                              .ReturnsAsync(discountCode);
                jsfServiceMock.Setup(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()))
                              .Callback<ConfirmPurchaseViewModel, string>((a, b) => { callBackUriCallback = b; })
                              .Returns(paymentInitiationResult);
                jsfServiceMock.Setup(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()))
                              .Callback<ConfirmPurchaseViewModel>((a) => { confirmPurchaseViewModelCallback = a; })
                              .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = purchaseId });

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void Checkout_ConfirmPurchaseViewModelNull_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.Checkout(null).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Never);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Never);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Settings.Default.ConfirmPurchaseViewModelNullErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void Checkout_GetPlansAsyncNull_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetPlansAsync())
                              .ReturnsAsync((IEnumerable<PlanViewModel>)null);

                // test
                var result = controller.Checkout(confirmPurchaseViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Once);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Never);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Settings.Default.FailedToRetrievePlansErrorMessage, result.RouteValues["errorMessage"]);

                // verify the session variables have been added
                Assert.AreEqual(0, sessionMock.Count);
            }

            [TestMethod]
            public void Checkout_InitiatePayPalPaymentFails_ReturnsRedirectToRouteResult()
            {
                // setup
                paymentInitiationResult.Success = false;
                paymentInitiationResult.ErrorMessage = "Failed to make payment";

                // test
                var result = controller.Checkout(confirmPurchaseViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Once);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Never);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.FailedToInitiatePayPalPaymentErrorMessage, paymentInitiationResult.ErrorMessage), result.RouteValues["errorMessage"]);

                // verify the session variables have been added
                Assert.AreEqual(0, sessionMock.Count);

                // verify the format of the callback uri
                Assert.AreEqual(string.Format(Settings.Default.CallbackUri, $"{requestScheme}://{requestUrl}"), callBackUriCallback);
            }

            [TestMethod]
            public void Checkout_SavePurchaseAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()))
                              .Callback<ConfirmPurchaseViewModel>((a) => { confirmPurchaseViewModelCallback = a; })
                              .ReturnsAsync(new AsyncResult<long>() { Success = false });


                // test
                var result = controller.Checkout(confirmPurchaseViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Once);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Once);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.FailedToSaveItemsForPurchase, emailAddress), result.RouteValues["errorMessage"]);

                Assert.IsNotNull(confirmPurchaseViewModelCallback);
                Assert.AreEqual(2, confirmPurchaseViewModelCallback.BasketItems.Count());
                Assert.AreEqual(planPrice1 - (planPrice1 / 100 * discountPercentage), confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId1).Price);
                Assert.AreEqual(planPrice2 - (planPrice2 / 100 * discountPercentage), confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId2).Price);
                Assert.AreEqual(paymentId, confirmPurchaseViewModelCallback.PayPalReference);
                Assert.AreEqual(transationId, confirmPurchaseViewModelCallback.TransactionId);

                // verify the session variables have been added
                Assert.AreEqual(2, sessionMock.Count);
                Assert.AreEqual(paymentId, sessionMock[SessionKeys.PaymentId]);
                Assert.AreEqual(transationId, sessionMock[SessionKeys.TransactionId]);

                // verify the format of the callback uri
                Assert.AreEqual(string.Format(Settings.Default.CallbackUri, $"{requestScheme}://{requestUrl}"), callBackUriCallback);
            }

            [TestMethod]
            public void Checkout_NullDiscountCode_ReturnsRedirectToRouteResult()
            {
                // setup
                confirmPurchaseViewModel.DiscountCodeId = null;
                jsfServiceMock.Setup(s => s.GetDiscountCodeAsync(It.IsAny<long>()))
                              .ReturnsAsync((DiscountCodeViewModel)null);

                // test
                var result = controller.Checkout(confirmPurchaseViewModel).Result as RedirectResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Once);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Once);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual(redirecturl, result.Url);

                Assert.IsNotNull(confirmPurchaseViewModelCallback);
                Assert.AreEqual(2, confirmPurchaseViewModelCallback.BasketItems.Count());
                Assert.AreEqual(planPrice1, confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId1).Price);
                Assert.AreEqual(planPrice2, confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId2).Price);
                Assert.AreEqual(paymentId, confirmPurchaseViewModelCallback.PayPalReference);
                Assert.AreEqual(transationId, confirmPurchaseViewModelCallback.TransactionId);

                // verify the session variables have been added
                Assert.AreEqual(3, sessionMock.Count);
                Assert.AreEqual(paymentId, sessionMock[SessionKeys.PaymentId]);
                Assert.AreEqual(transationId, sessionMock[SessionKeys.TransactionId]);
                Assert.AreEqual(purchaseId, sessionMock[SessionKeys.PurchaseId]);

                // verify the format of the callback uri
                Assert.AreEqual(string.Format(Settings.Default.CallbackUri, $"{requestScheme}://{requestUrl}"), callBackUriCallback);
            }

            [TestMethod]
            public void Checkout_AppliesDiscountCode_ReturnsRedirectResult()
            {
                // test
                var result = controller.Checkout(confirmPurchaseViewModel).Result as RedirectResult;

                // verify
                jsfServiceMock.Verify(s => s.GetPlansAsync(), Times.Once);
                jsfServiceMock.Verify(s => s.GetDiscountCodeAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.InitiatePayPalPayment(It.IsAny<ConfirmPurchaseViewModel>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.SavePurchaseAsync(It.IsAny<ConfirmPurchaseViewModel>()), Times.Once);

                // verify the result is correct
                Assert.IsNotNull(result);
                Assert.AreEqual(redirecturl, result.Url);

                Assert.IsNotNull(confirmPurchaseViewModelCallback);
                Assert.AreEqual(2, confirmPurchaseViewModelCallback.BasketItems.Count());
                Assert.AreEqual(planPrice1 - (planPrice1 / 100 * discountPercentage), confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId1).Price);
                Assert.AreEqual(planPrice2 - (planPrice2 / 100 * discountPercentage), confirmPurchaseViewModelCallback.BasketItems.First(b => b.PlanId == planId2).Price);
                Assert.AreEqual(paymentId, confirmPurchaseViewModelCallback.PayPalReference);
                Assert.AreEqual(transationId, confirmPurchaseViewModelCallback.TransactionId);

                // verify the session variables have been added
                Assert.AreEqual(3, sessionMock.Count);
                Assert.AreEqual(paymentId, sessionMock[SessionKeys.PaymentId]);
                Assert.AreEqual(transationId, sessionMock[SessionKeys.TransactionId]);
                Assert.AreEqual(purchaseId, sessionMock[SessionKeys.PurchaseId]);

                // verify the format of the callback uri
                Assert.AreEqual(string.Format(Settings.Default.CallbackUri, $"{requestScheme}://{requestUrl}"), callBackUriCallback);
            }
        }
    }
}
