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
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    public partial class HomeController
    {
        [TestClass]
        public class ConfirmPurchase
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
            ItemQuantityViewModel itemQuantityViewModel1;
            ItemQuantityViewModel itemQuantityViewModel2;
            Dictionary<long, ItemQuantityViewModel> sessionBasketItems;
            SelectedPlanOptionViewModel selectedPlanOptionViewModel1;
            SelectedPlanOptionViewModel selectedPlanOptionViewModel2;
            SelectedPlanOptionViewModel selectedPlanOptionViewModel3;
            IEnumerable<SelectedPlanOptionViewModel> selectedPlanOptionViewModels;
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
                
                itemQuantityViewModel1 = new ItemQuantityViewModel() { Id = basketItemId1, Quantity = basketItemQuantity1 };
                itemQuantityViewModel2 = new ItemQuantityViewModel() { Id = basketItemId2, Quantity = basketItemQuantity2 };

                sessionBasketItems = new Dictionary<long, ItemQuantityViewModel>()
                {
                    { basketItemId1, itemQuantityViewModel1 },
                    { basketItemId2, itemQuantityViewModel2 },
                };

                selectedPlanOptionViewModel1 = new SelectedPlanOptionViewModel() { Id = basketItemId1 };
                selectedPlanOptionViewModel2 = new SelectedPlanOptionViewModel() { Id = basketItemId2 };
                selectedPlanOptionViewModel3 = new SelectedPlanOptionViewModel() { Id = basketItemId3 };

                selectedPlanOptionViewModels = new List<SelectedPlanOptionViewModel>()
                {
                    selectedPlanOptionViewModel1,
                    selectedPlanOptionViewModel2,
                    selectedPlanOptionViewModel3,
                };

                customer = new CustomerViewModel()
                {
                    Id = customerId,
                };
                
                basketHelperMock.Setup(b => b.GetBasketItems())
                                .Returns(sessionBasketItems);

                jsfServiceMock.Setup(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()))
                              .ReturnsAsync(selectedPlanOptionViewModels);

                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(customer);
                
                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void ConfirmPurchase_CustomerIdNull_ReturnRedirectToRouteResult()
            {
                // test
                var result = controller.ConfirmPurchase(Guid.Empty).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Never);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);
                
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.CustomerIdNullErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmPurchase_GetBasketItemsFails_ReturnsRedirectToRouteResult()
            {
                // setup
                basketHelperMock.Setup(b => b.GetBasketItems())
                                .Returns((IDictionary<long, ItemQuantityViewModel>)null);

                // test
                var result = controller.ConfirmPurchase(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Once);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.BasketItemsNullErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmPurchase_GetBasketItemsAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()))
                              .ReturnsAsync((IEnumerable<SelectedPlanOptionViewModel>)null);

                // test
                var result = controller.ConfirmPurchase(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Once);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.BasketItemsAsyncNullErrorMessage, customerId, string.Join(",", sessionBasketItems.Keys.ToList())), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmPurchase_GetCustomerDetailsAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((CustomerViewModel)null);

                // test
                var result = controller.ConfirmPurchase(customerId).Result as RedirectToRouteResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Once);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Settings.Default.GetCustomerDetailsAsyncErrorMessage, customerId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void ConfirmPurchase_ApplyDiscountCode_ReturnsView()
            {
                // test
                var result = controller.ConfirmPurchase(customerId).Result as ViewResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Once);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);

                var resultModel = (ConfirmPurchaseViewModel)result.Model;

                // assert customer details
                Assert.IsNotNull(resultModel.CustomerDetails);
                Assert.AreEqual(customerId, resultModel.CustomerDetails.Id);

                // assert basket item details
                Assert.IsNotNull(resultModel.BasketItems);
                Assert.AreEqual(3, resultModel.BasketItems.Count());
                Assert.AreEqual(basketItemQuantity1, resultModel.BasketItems.First(b => b.Id == basketItemId1).Quantity);
                Assert.AreEqual(basketItemQuantity2, resultModel.BasketItems.First(b => b.Id == basketItemId2).Quantity);

                // check item is set to default quantity
                Assert.AreEqual(basketItemDefaultQuantity, resultModel.BasketItems.First(b => b.Id == basketItemId3).Quantity);

                // verify discount code is present
                Assert.IsNotNull(resultModel.DiscountCode);
                Assert.AreEqual(discountCodeId, resultModel.DiscountCodeId);
            }

            [TestMethod]
            public void ConfirmPurchase_NoDiscountCode_ReturnsView()
            {
                // setup
                sessionMock[SessionKeys.DiscountCode] = null;

                // test
                var result = controller.ConfirmPurchase(customerId).Result as ViewResult;

                // verify
                basketHelperMock.Verify(b => b.GetBasketItems(), Times.Once);
                jsfServiceMock.Verify(s => s.GetBasketItemsAsync(It.IsAny<IEnumerable<long>>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);

                var resultModel = (ConfirmPurchaseViewModel)result.Model;

                // assert customer details
                Assert.IsNotNull(resultModel.CustomerDetails);
                Assert.AreEqual(customerId, resultModel.CustomerDetails.Id);

                // assert basket item details
                Assert.IsNotNull(resultModel.BasketItems);
                Assert.AreEqual(3, resultModel.BasketItems.Count());
                Assert.AreEqual(basketItemQuantity1, resultModel.BasketItems.First(b => b.Id == basketItemId1).Quantity);
                Assert.AreEqual(basketItemQuantity2, resultModel.BasketItems.First(b => b.Id == basketItemId2).Quantity);
                
                // check item is set to default quantity
                Assert.AreEqual(basketItemDefaultQuantity, resultModel.BasketItems.First(b => b.Id == basketItemId3).Quantity);

                // verify discount code is null
                Assert.IsNull(resultModel.DiscountCode);
            }
        }
    }
}
