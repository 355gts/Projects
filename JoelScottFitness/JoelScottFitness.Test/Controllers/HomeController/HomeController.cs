using JoelScottFitness.Common.IO;
using JoelScottFitness.Services.Services;
using JoelScottFitness.Web.Helpers;
using JoelScottFitness.YouTube.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CON = JoelScottFitness.Web.Controllers;

namespace JoelScottFitness.Test.Controllers.HomeController
{
    [TestClass]
    public partial class HomeController
    {
        [TestClass]
        public class ConstructorTests
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_JsfServiceNull_ThrowsNullArgumentException()
            {
                new CON.HomeController(null,
                                       new Mock<IYouTubeClient>().Object,
                                       new Mock<IBasketHelper>().Object,
                                       new Mock<IFileHelper>().Object);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_YouTubeClientNull_ThrowsNullArgumentException()
            {
                new CON.HomeController(new Mock<IJSFitnessService>().Object,
                                       null,
                                       new Mock<IBasketHelper>().Object,
                                       new Mock<IFileHelper>().Object);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_BasketHelperNull_ThrowsNullArgumentException()
            {
                new CON.HomeController(new Mock<IJSFitnessService>().Object,
                                       new Mock<IYouTubeClient>().Object,
                                       null,
                                       new Mock<IFileHelper>().Object);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void Constructor_FileHelperNull_ThrowsNullArgumentException()
            {
                new CON.HomeController(new Mock<IJSFitnessService>().Object,
                                       new Mock<IYouTubeClient>().Object,
                                       new Mock<IBasketHelper>().Object,
                                       null);
            }
        }
    }
}
