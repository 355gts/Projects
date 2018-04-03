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
using System.Web.Mvc;
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

            long purchaseId = 111;
            long questionnaireId = 222;
            QuestionnaireViewModel questionnaireViewModel;

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

                questionnaireViewModel = new QuestionnaireViewModel()
                {
                    OrderId = purchaseId,
                };

                jsfServiceMock.Setup(s => s.CreateOrUpdateQuestionnaireAsync(It.IsAny<QuestionnaireViewModel>()))
                              .ReturnsAsync(new AsyncResult<long>() { Success = true, Result = questionnaireId });

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
                Assert.AreEqual(string.Format(Settings.Default.FailedToCreateOrUpdateQuestionnaireErrorMessage, purchaseId), result.RouteValues["errorMessage"]);
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
                Assert.AreEqual(Settings.Default.QuestionnaireCompleteConfirmationMessage, result.ViewData["Message"]);
            }
        }
    }
}
