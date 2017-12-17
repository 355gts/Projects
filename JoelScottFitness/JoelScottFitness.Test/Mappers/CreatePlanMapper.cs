using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreatePlanMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreatePlanViewModel()
                {
                    BannerHeader = "Test Banner",
                    Description = "Test Description",
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<CreatePlanOptionViewModel>(),
                    BannerColour = BannerColour.White,
                };

                var mapper = new Map.CreatePlanMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreatePlanViewModel()
                {
                    BannerHeader = "Test Banner",
                    Description = "Test Description",
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<CreatePlanOptionViewModel>(),
                    BannerColour = BannerColour.White,
                };

                Plan toObject = new Plan();

                var mapper = new Map.CreatePlanMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreatePlanViewModel webObject, Plan repoObject)
            {
                Assert.AreEqual(repoObject.BannerHeader, webObject.BannerHeader);
                Assert.IsNotNull(repoObject.CreatedDate);
                Assert.IsNull(repoObject.ModifiedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(0, repoObject.Id);
                Assert.AreEqual(repoObject.ImagePathLarge, webObject.ImagePathLarge);
                Assert.AreEqual(repoObject.ImagePathMedium, webObject.ImagePathMedium);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.TargetGender, webObject.TargetGender);
                Assert.AreEqual(repoObject.BannerColour, webObject.BannerColour);
                Assert.AreEqual(0, webObject.Options.Count);
            }
        }
    }
}
