using JoelScottFitness.Common.IO;
using JoelScottFitness.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.AdminController
{
    [TestClass]
    public partial class AdminController
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_JsfServiceNull_ThrowsNullArgumentException()
            {
                new CON.AdminController(null,
                                        new Mock<IFileHelper>().Object);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_FileHelperNull_ThrowsNullArgumentException()
            {
                new CON.AdminController(new Mock<IJSFitnessService>().Object,
                                        null);
            }
        }
    }
}
