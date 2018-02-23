using JoelScottFitness.Data;
using JoelScottFitness.PayPal.Services;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using SRV = JoelScottFitness.Services.Services;

namespace JoelScottFitness.Test.Services
{
    public partial class JSFitnessService
    {
        [TestClass]
        public class GetCustomerPlansAsync
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


                service = new SRV.JSFitnessService(repositoryMock.Object,
                                         MapperHelper.GetServiceMapper(),
                                         paypalServiceMock.Object,
                                         emailServiceMock.Object);
            }

            [TestMethod]
            public void GetCustomerPlansAsync_Success()
            {
                throw new NotImplementedException();
            }

        }
    }
}
