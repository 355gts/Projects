using JoelScottFitness.Common.IO;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using JoelScottFitness.Web.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.AdminController
{
    public partial class AdminController
    {
        [TestClass]
        public class Blogs
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;
            Mock<HttpPostedFileBase> fileMock;

            string uploadPath = "uploadPath";
            string uploadPathCallback;
            string uploadFilename;
            string planPath1 = "planPath1";
            string planPath2 = "planPath2";
            CustomerViewModel customerViewModel;
            PurchaseHistoryViewModel purchaseHistoryViewModel;
            OrderItemViewModel purchasedHistoryItemViewModel1;
            OrderItemViewModel purchasedHistoryItemViewModel2;
            UploadPlanViewModel uploadPlanViewModel;
            string requestUrl = "requesturl";
            string requestScheme = "https";
            string emailAddress = "emailAddress";
            string postedFileName = "postedFileName";
            long purchaseId = 123;
            string transactionId = "transactionId";
            Guid customerId = Guid.NewGuid();
            RouteData routeData = new RouteData();
            Mock<IView> viewMock;
            Mock<IViewEngine> engineMock;
            ViewEngineResult viewEngineResultMock;
            IEnumerable<string> emailAddressesCallback;
            string emailSubjectCallback;

            CON.AdminController controller;

            [TestInitialize]
            public void TestSetup()
            {
                jsfServiceMock = new Mock<IJSFitnessService>();
                fileHelperMock = new Mock<IFileHelper>();
                contextMock = new Mock<ControllerContext>();
                sessionMock = new MockHttpSessionBase();
                requestMock = new Mock<HttpRequestBase>();
                fileMock = new Mock<HttpPostedFileBase>();

                routeData = new RouteData();
                routeData.Values.Add("HomeController", "_OrderComplete");

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.SetupGet(m => m.RouteData)
                           .Returns(routeData);
                contextMock.SetupGet(m => m.HttpContext.Request)
                           .Returns(requestMock.Object);

                fileMock.Setup(f => f.FileName).Returns(postedFileName);

                viewMock = new Mock<IView>();
                engineMock = new Mock<IViewEngine>();
                viewEngineResultMock = new ViewEngineResult(viewMock.Object, engineMock.Object);
                engineMock.Setup(e => e.FindPartialView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(viewEngineResultMock);
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(engineMock.Object);

                customerViewModel = new CustomerViewModel()
                {
                    Firstname = "Firstname",
                    Surname = "Surname",
                    EmailAddress = emailAddress,
                };

                uploadPlanViewModel = new UploadPlanViewModel()
                {
                    CustomerId = customerId,
                    Name = "Name",
                    Description = "Description",
                    PurchaseId = purchaseId,
                    PostedFile = fileMock.Object,
                };

                purchasedHistoryItemViewModel1 = new OrderItemViewModel() { /*PlanPath = planPath1,*/ RequiresAction = false };
                purchasedHistoryItemViewModel2 = new OrderItemViewModel() { /*PlanPath = planPath2,*/ RequiresAction = false };
                purchaseHistoryViewModel = new PurchaseHistoryViewModel()
                {
                    Customer = customerViewModel,
                    TransactionId = transactionId,
                    Items = new List<OrderItemViewModel>()
                    {
                        purchasedHistoryItemViewModel1,
                        purchasedHistoryItemViewModel2,
                    },
                };

                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(customerViewModel);
                jsfServiceMock.Setup(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()))
                              .Callback<long, string>((a, b) => { uploadPathCallback = b; })
                              .ReturnsAsync(true);
                jsfServiceMock.Setup(s => s.GetPurchaseAsync(It.IsAny<long>()))
                              .ReturnsAsync(purchaseHistoryViewModel);
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                              .Callback<string, string, IEnumerable<string>, IEnumerable<string>>((a, b, c, d) => { emailSubjectCallback = a; emailAddressesCallback = c; })
                              .ReturnsAsync(true);

                fileHelperMock.Setup(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Callback<HttpPostedFileBase, string, string>((a, b, c) => { uploadFilename = c; })
                              .Returns(new UploadResult() { Success = true, UploadPath = uploadPath });
                fileHelperMock.Setup(f => f.MapPath(It.IsAny<string>()))
                              .Returns(uploadPath);

                controller = new CON.AdminController(jsfServiceMock.Object,
                                                     fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void UploadPlan_ModelStateInvalid_ReturnsViewResult()
            {
                // setup
                controller.ModelState.AddModelError(string.Empty, "Error");

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Never);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual("Error", controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void UploadPlan_GetCustomerDetailsAsyncFails_ReturnsViewResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((CustomerViewModel)null);

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(string.Format(Settings.Default.FailedToFindCustomerErrorMessage, uploadPlanViewModel.CustomerId), controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void UploadPlan_UploadFileFails_ReturnsViewResult()
            {
                // setup
                fileHelperMock.Setup(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()))
                              .Returns(new UploadResult() { Success = false });

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Never);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(2, controller.ModelState.Values.First().Errors.Count());
                Assert.AreEqual(1, controller.ModelState.Values.First().Errors.Count(e => e.ErrorMessage == string.Format(Settings.Default.FailedToUploadPlanForCustomerErrorMessage, uploadPlanViewModel.PurchaseId, uploadPlanViewModel.CustomerId)));
                Assert.AreEqual(1, controller.ModelState.Values.First().Errors.Count(e => e.ErrorMessage == string.Format(Settings.Default.FailedToUploadFileErrorMessage, postedFileName)));

            }

            [TestMethod]
            public void UploadPlan_AssociatePlanToPurchaseAsyncFails_ReturnsViewResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()))
                              .ReturnsAsync(false);

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Never);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(string.Format(Settings.Default.FailedToAssociatePlanToPurchaseErrorMessage, uploadPath, uploadPlanViewModel.PurchaseId, uploadPlanViewModel.CustomerId), controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void UploadPlan_GetPurchaseAsyncFails_ReturnsViewResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.GetPurchaseAsync(It.IsAny<long>()))
                              .ReturnsAsync((PurchaseHistoryViewModel)null);

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(string.Format(Settings.Default.FailedToAssociatePlanToPurchaseErrorMessage, uploadPath, uploadPlanViewModel.PurchaseId, uploadPlanViewModel.CustomerId), controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void UploadPlan_SendOrderCompleteEmailFails_ReturnsViewResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()))
                              .ReturnsAsync(false);

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as ViewResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Exactly(purchaseHistoryViewModel.Items.Count()));

                Assert.IsNotNull(result);
                Assert.AreEqual(1, controller.ModelState.Count());
                Assert.IsFalse(controller.ModelState.IsValid);
                Assert.AreEqual(string.Format(Settings.Default.FailedToSendOrderCompleteEmailErrorMessage, uploadPlanViewModel.PurchaseId, uploadPlanViewModel.CustomerId), controller.ModelState.Values.First().Errors.First().ErrorMessage);
            }

            [TestMethod]
            public void UploadPlan_PurchaseViewModelContainsEmptyList_ReturnsRedirectToRouteResult()
            {
                // setup
                purchaseHistoryViewModel.Items.Remove(purchasedHistoryItemViewModel1);
                purchaseHistoryViewModel.Items.Remove(purchasedHistoryItemViewModel2);

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("CustomerPlan", result.RouteValues["action"]);
                Assert.AreEqual("Admin", result.RouteValues["controller"]);
                Assert.AreEqual(purchaseId, result.RouteValues["purchaseId"]);

                // verify the uploaded file name
                Assert.AreEqual(string.Format(Settings.Default.PlanFilenameFormat, customerViewModel.Firstname, customerViewModel.Surname, uploadPlanViewModel.Name, uploadPlanViewModel.Description, DateTime.UtcNow.ToString("yyyyMMdd")), uploadFilename);
            }

            [TestMethod]
            public void UploadPlan_PurchaseViewModelItemsNull_ReturnsRedirectToRouteResult()
            {
                // setup
                purchaseHistoryViewModel.Items = null;

                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Never);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Never);

                Assert.IsNotNull(result);
                Assert.AreEqual("CustomerPlan", result.RouteValues["action"]);
                Assert.AreEqual("Admin", result.RouteValues["controller"]);
                Assert.AreEqual(purchaseId, result.RouteValues["purchaseId"]);

                // verify the uploaded file name
                Assert.AreEqual(string.Format(Settings.Default.PlanFilenameFormat, customerViewModel.Firstname, customerViewModel.Surname, uploadPlanViewModel.Name, uploadPlanViewModel.Description, DateTime.UtcNow.ToString("yyyyMMdd")), uploadFilename);
            }

            [TestMethod]
            public void UploadPlan_Success_ReturnsRedirectToRouteResult()
            {
                // test
                var result = controller.UploadPlan(uploadPlanViewModel).Result as RedirectToRouteResult;

                // verify
                jsfServiceMock.Verify(s => s.GetCustomerDetailsAsync(It.IsAny<Guid>()), Times.Once);
                jsfServiceMock.Verify(s => s.UploadCustomerPlanAsync(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
                jsfServiceMock.Verify(s => s.GetPurchaseAsync(It.IsAny<long>()), Times.Once);
                jsfServiceMock.Verify(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<string>>()), Times.Once);

                fileHelperMock.Verify(f => f.UploadFile(It.IsAny<HttpPostedFileBase>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                fileHelperMock.Verify(f => f.MapPath(It.IsAny<string>()), Times.Exactly(purchaseHistoryViewModel.Items.Count()));

                Assert.IsNotNull(result);
                Assert.AreEqual("CustomerPlan", result.RouteValues["action"]);
                Assert.AreEqual("Admin", result.RouteValues["controller"]);
                Assert.AreEqual(purchaseId, result.RouteValues["purchaseId"]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallback);
                Assert.AreEqual(1, emailAddressesCallback.Count());
                Assert.AreEqual(emailAddress, emailAddressesCallback.First());
                Assert.IsNotNull(emailSubjectCallback);
                Assert.AreEqual(string.Format(Settings.Default.PurchaseComplete, transactionId), emailSubjectCallback);

                // verify the uploaded file name
                Assert.AreEqual(string.Format(Settings.Default.PlanFilenameFormat, customerViewModel.Firstname, customerViewModel.Surname, uploadPlanViewModel.Name, uploadPlanViewModel.Description, DateTime.UtcNow.ToString("yyyyMMdd")), uploadFilename);
            }
        }
    }
}
