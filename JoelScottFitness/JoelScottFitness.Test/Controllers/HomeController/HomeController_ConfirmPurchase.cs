using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
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
using System.Web;
using System.Web.Mvc;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class ConfirmOrder
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            Guid customerId = Guid.NewGuid();
            string payerId = "payerId";
            long basketItemId1 = 111;
            long basketItemId2 = 222;
            long basketItemId3 = 333;
            long basketItemQuantity1 = 3;
            long basketItemQuantity2 = 2;
            long basketItemDefaultQuantity = 1;
            BasketItemViewModel BasketItemViewModel1;
            BasketItemViewModel BasketItemViewModel2;
            BasketViewModel sessionBasket;
            BasketItemViewModel basketItemViewModel1;
            BasketItemViewModel basketItemViewModel2;
            BasketItemViewModel basketItemViewModel3;
            IDictionary<long, BasketItemViewModel> basketItemViewModels;
            BasketViewModel basketViewModel;
            CustomerViewModel customer;
            long discountCodeId = 555;

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

                requestMock.SetupGet(r => r.Params)
                           .Returns(new NameValueCollection() { { SessionKeys.PayerId, payerId } });

                sessionMock.Add(SessionKeys.ConfirmOrderViewModel, new ConfirmOrderViewModel());

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                BasketItemViewModel1 = new BasketItemViewModel() { Id = basketItemId1, Quantity = basketItemQuantity1 };
                BasketItemViewModel2 = new BasketItemViewModel() { Id = basketItemId2, Quantity = basketItemQuantity2 };

                sessionBasket = new BasketViewModel()
                {
                    Items = new Dictionary<long, BasketItemViewModel>()
                    {
                        { basketItemId1, BasketItemViewModel1 },
                        { basketItemId2, BasketItemViewModel2 },
                    }
                };

                basketItemViewModel1 = new BasketItemViewModel() { Id = basketItemId1 };
                basketItemViewModel2 = new BasketItemViewModel() { Id = basketItemId2 };
                basketItemViewModel3 = new BasketItemViewModel() { Id = basketItemId3 };

                basketItemViewModels = new Dictionary<long, BasketItemViewModel>()
                {
                    { basketItemId1, basketItemViewModel1 },
                    { basketItemId2, basketItemViewModel2 },
                    { basketItemId3, basketItemViewModel3 },
                };

                basketViewModel = new BasketViewModel()
                {
                    Items = basketItemViewModels
                };

                customer = new CustomerViewModel()
                {
                    Id = customerId,
                };

                basketHelperMock.Setup(b => b.GetBasket())
                                .Returns(sessionBasket);

                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(customer);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void ConfirmOrder_PayerIdNull_RedirectsToError()
            {
                // setup
                requestMock.SetupGet(r => r.Params)
                           .Returns(new NameValueCollection() { { SessionKeys.PayerId, null } });

                // test
                var result = controller.ConfirmOrder() as RedirectToRouteResult;

                // verify
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.PayerIdNullErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmOrder_ConfirmOrderViewModelNull_RedirectsToError()
            {
                // setup
                sessionMock.Remove(SessionKeys.ConfirmOrderViewModel);

                // test
                var result = controller.ConfirmOrder() as RedirectToRouteResult;

                // verify
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(Resources.ConfirmOrderViewModelNullErrorMessage, result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmOrder_Success_ReturnsView()
            {
                // test
                var result = controller.ConfirmOrder() as ViewResult;

                // verify
                Assert.IsNotNull(result);
                var confirmOrderViewModel = (ConfirmOrderViewModel)result.Model;
                Assert.IsNotNull(confirmOrderViewModel);

                // verify the session variables have been added
                Assert.AreEqual(2, sessionMock.Count);
                Assert.IsNotNull((string)sessionMock[SessionKeys.PayerId]);
                Assert.IsNotNull((ConfirmOrderViewModel)sessionMock[SessionKeys.ConfirmOrderViewModel]);
            }
        }
    }
}
