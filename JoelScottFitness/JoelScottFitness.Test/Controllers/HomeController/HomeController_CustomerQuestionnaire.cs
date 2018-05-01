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
        public class CustomerQuestionnaire
        {
            Mock<IJSFitnessService> jsfServiceMock;
            Mock<IYouTubeClient> youtubeClientMock;
            Mock<IBasketHelper> basketHelperMock;
            Mock<IFileHelper> fileHelperMock;
            Mock<ControllerContext> contextMock;
            MockHttpSessionBase sessionMock;
            Mock<HttpRequestBase> requestMock;

            long purchaseId = 111;
            long questionnaireId = 222;
            QuestionnaireViewModel questionnaireViewModel;
            string transactionId = "transactionId";
            string requestUrl = "requesturl";
            string requestScheme = "https";
            string emailAddress = "emailAddress";
            RouteData routeData = new RouteData();
            Mock<IView> viewMock;
            Mock<IViewEngine> engineMock;
            ViewEngineResult viewEngineResultMock;
            List<string> emailAddressesCallbacks;
            List<string> emailSubjectCallbacks;

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
                routeData.Values.Add("HomeController", "_QuestionnaireComplete");
                contextMock.SetupGet(m => m.RouteData).Returns(routeData);

                viewMock = new Mock<IView>();
                engineMock = new Mock<IViewEngine>();
                viewEngineResultMock = new ViewEngineResult(viewMock.Object, engineMock.Object);
                engineMock.Setup(e => e.FindPartialView(It.IsAny<ControllerContext>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(viewEngineResultMock);
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(engineMock.Object);

                questionnaireViewModel = new QuestionnaireViewModel()
                {
                    TransactionId = transactionId,
                    OrderId = purchaseId,
                };

                requestMock.SetupGet(r => r.Url)
                           .Returns(new Uri($"{requestScheme}://{requestUrl}", UriKind.Absolute));

                contextMock.Setup(c => c.HttpContext.Session)
                           .Returns(sessionMock);
                contextMock.Setup(c => c.HttpContext.Request)
                           .Returns(requestMock.Object);

                emailAddressesCallbacks = new List<string>();
                emailSubjectCallbacks = new List<string>();

                jsfServiceMock.Setup(s => s.CreateOrUpdateQuestionnaireAsync(It.IsAny<QuestionnaireViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = questionnaireId });
                jsfServiceMock.Setup(s => s.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                              .Callback<string, string, IEnumerable<string>>((a, b, c) => { emailSubjectCallbacks.Add(a); emailAddressesCallbacks.AddRange(c); })
                              .ReturnsAsync(true);

                controller = new CON.HomeController(jsfServiceMock.Object,
                                                    youtubeClientMock.Object,
                                                    basketHelperMock.Object,
                                                    fileHelperMock.Object);

                controller.ControllerContext = contextMock.Object;
            }

            [TestMethod]
            public void CustomerQuestionnaire_CreateOrUpdateQuestionnaireAsyncFails_ReturnsRedirectToRouteResult()
            {
                // setup
                jsfServiceMock.Setup(s => s.CreateOrUpdateQuestionnaireAsync(It.IsAny<QuestionnaireViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = false });

                // test
                var result = controller.CustomerQuestionnaire(questionnaireViewModel).Result as RedirectToRouteResult;

                // verify
                Assert.IsNotNull(result);
                Assert.AreEqual("Error", result.RouteValues["action"]);
                Assert.AreEqual("Home", result.RouteValues["controller"]);
                Assert.AreEqual(string.Format(Resources.FailedToCreateOrUpdateQuestionnaireErrorMessage, purchaseId), result.RouteValues["errorMessage"]);
            }

            [TestMethod]
            public void CustomerQuestionnaire_Success_ReturnsViewResult()
            {
                // test
                var result = controller.CustomerQuestionnaire(questionnaireViewModel).Result as ViewResult;

                // verify
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Model);
                var resultModel = (QuestionnaireViewModel)result.Model;
                Assert.AreEqual(typeof(QuestionnaireViewModel), resultModel.GetType());
                Assert.AreEqual(Resources.QuestionnaireCompleteConfirmationMessage, result.ViewData["Message"]);

                // verify email parameters
                Assert.IsNotNull(emailAddressesCallbacks);
                Assert.AreEqual(1, emailAddressesCallbacks.Count());
                Assert.AreEqual(Settings.Default.EmailAddress, emailAddressesCallbacks.First());
                Assert.IsNotNull(emailSubjectCallbacks);
                Assert.AreEqual(string.Format(Resources.QuestionnaireComplete, transactionId), emailSubjectCallbacks.First());
            }
        }
    }
}
