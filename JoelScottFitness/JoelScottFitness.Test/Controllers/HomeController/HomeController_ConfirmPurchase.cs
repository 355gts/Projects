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
using System.Linq;
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

            Guid customerId = Guid.NewGuid();
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
            DiscountCodeViewModel discountCode;
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

                discountCode = new DiscountCodeViewModel() { Id = discountCodeId };

                sessionMock[SessionKeys.DiscountCode] = discountCode;

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);

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
            public void ConfirmOrder_CustomerIdNull_ReturnRedirectToRouteResult()
            {
                // test
                var result = controller.ConfirmOrder() as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Never);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.CustomerIdNullErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmOrder_GetBasketFails_ReturnsRedirectToRouteResult()
            {
                // setup
                basketHelperMock.Setup(b => b.GetBasket())
                                .Returns((BasketViewModel)null);

                // test
                var result = controller.ConfirmOrder() as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.BasketItemsNullErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmOrder_GetCustomerDetailsAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((CustomerViewModel)null);

                // test
                var result = controller.ConfirmOrder() as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.GetCustomerDetailsAsyncErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmOrder_ApplyDiscountCode_ReturnsView()
            {
                // test
                var result = controller.ConfirmOrder() as ViewResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);

                var resultModel = (ConfirmOrderViewModel)result.Model;

                // assert customer details
                Assert.IsNotNull(resultModel.CustomerDetails);
                Assert.AreEqual(customerId, resultModel.CustomerDetails.Id);

                // assert basket item details
                Assert.IsNotNull(resultModel.Basket.Items);
                Assert.AreEqual(3, resultModel.Basket.Items.Count());
                Assert.AreEqual(basketItemQuantity1, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId1).Value.Quantity);
                Assert.AreEqual(basketItemQuantity2, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId2).Value.Quantity);

                // check item is set to default quantity
                Assert.AreEqual(basketItemDefaultQuantity, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId3).Value.Quantity);

                // verify discount code is present
                Assert.IsNotNull(resultModel.Basket.DiscountCode);
                Assert.AreEqual(discountCodeId, resultModel.Basket.DiscountCode.Id);
            }

            [TestMethod]
            public void ConfirmOrder_NoDiscountCode_ReturnsView()
            {
                // setup
                sessionMock[SessionKeys.DiscountCode] = null;

                // test
                var result = controller.ConfirmOrder() as ViewResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasket(), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);

                var resultModel = (ConfirmOrderViewModel)result.Model;

                // assert customer details
                Assert.IsNotNull(resultModel.CustomerDetails);
                Assert.AreEqual(customerId, resultModel.CustomerDetails.Id);

                // assert basket item details
                Assert.IsNotNull(resultModel.Basket.Items);
                Assert.AreEqual(3, resultModel.Basket.Items.Count());
                Assert.AreEqual(basketItemQuantity1, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId1).Value.Quantity);
                Assert.AreEqual(basketItemQuantity2, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId2).Value.Quantity);

                // check item is set to default quantity
                Assert.AreEqual(basketItemDefaultQuantity, resultModel.Basket.Items.First(b => b.Value.Id == basketItemId3).Value.Quantity);

                // verify discount code is null
                Assert.IsNull(resultModel.Basket.DiscountCode);
            }
        }
    }
}
