using JoelScottFitness.Data;
using JoelScottFitness.Data.Models;
using JoelScottFitness.PayPal.Services;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using SRV = JoelScottFitness.Services.Services;

namespace JoelScottFitness.Test.Services
{
    public partial class JSFitnessService
    {
        [TestClass]
        public class GetPurchaseAsync
        {
            Mock<IJSFitnessRepository> repositoryMock;
            Mock<IPayPalService> paypalServiceMock;
            Mock<IEmailService> emailServiceMock;

            string userName = "userName";
            Guid customerId = Guid.NewGuid();
            string idString = "id";
            long id = 1234;

            SRV.JSFitnessService service;

            [TestInitialize]
            public void TestSetup()
            {
                repositoryMock = new Mock<IJSFitnessRepository>();
                paypalServiceMock = new Mock<IPayPalService>();
                emailServiceMock = new Mock<IEmailService>();

                repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                              .ReturnsAsync(new List<Purchase>() { new Purchase(), new Purchase() });
                repositoryMock.Setup(r => r.GetPurchasesAsync())
                              .ReturnsAsync(new List<Purchase>() { new Purchase(), new Purchase() });


                service = new SRV.JSFitnessService(repositoryMock.Object,
                                         MapperHelper.GetServiceMapper(),
                                         paypalServiceMock.Object,
                                         emailServiceMock.Object);
            }

            [TestMethod]
            public void GetPurchaseAsync_Success()
            {
                throw new NotImplementedException();
            }

            [TestMethod]
            public void GetPurchaseSummaryAsync_NotFound_ReturnsEmptyList()
            {
                // setup
                repositoryMock.Setup(r => r.GetPurchasesAsync(It.IsAny<Guid>()))
                              .ReturnsAsync((IEnumerable<Purchase>)null);

                // test
                var result = service.GetPurchaseSummaryAsync(customerId).Result;

                // verify
                repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GetPurchaseSummaryAsync_Success_ReturnsPurchases()
            {
                // test
                var result = service.GetPurchaseSummaryAsync(customerId).Result;

                // verify
                repositoryMock.Verify(r => r.GetPurchasesAsync(It.IsAny<Guid>()), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
            }

            [TestMethod]
            public void GetPurchaseAsync_NotFound_ReturnsEmptyList()
            {
                // setup
                repositoryMock.Setup(r => r.GetPurchasesAsync())
                              .ReturnsAsync((IEnumerable<Purchase>)null);

                // test
                var result = service.GetPurchasesAsync().Result;

                // verify
                repositoryMock.Verify(r => r.GetPurchasesAsync(), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GetPurchaseAsync_Success_ReturnsPurchases()
            {
                // test
                var result = service.GetPurchasesAsync().Result;

                // verify
                repositoryMock.Verify(r => r.GetPurchasesAsync(), Times.Once);

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
            }
        }
    }
}
