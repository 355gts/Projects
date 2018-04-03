using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class PlanMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            Plan repoObject = new Plan()
            {
                Active = true,
                BannerHeader = "Test Banner",
                CreatedDate = new DateTime(2017, 11, 11, 19, 03, 32),
                Description = "Test Description",
                Id = 123,
                ImagePathLarge = "Image Large",
                ModifiedDate = new DateTime(2017, 12, 13, 19, 03, 32),
                Name = "Test Name",
                TargetGender = Gender.Female,
                Options = new List<PlanOption>(),
                BannerColour = BannerColour.White,
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.PlanMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                PlanViewModel toObject = new PlanViewModel();

                var mapper = new Map.PlanMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Plan repoObject, PlanViewModel webObject)
            {
                Assert.AreEqual(repoObject.Active, webObject.Active);
                Assert.AreEqual(repoObject.BannerColour, webObject.BannerColour);
                Assert.AreEqual(repoObject.BannerHeader, webObject.BannerHeader);
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePathLarge, webObject.ImagePathLarge);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.TargetGender, webObject.TargetGender);
                Assert.IsNotNull(webObject.ModifiedDate);
                Assert.AreEqual(0, webObject.Options.Count);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            PlanViewModel webObject = new PlanViewModel()
            {
                BannerHeader = "Test Banner",
                CreatedDate = DateTime.UtcNow,
                Description = "Test Description",
                Id = 123,
                ImagePathLarge = "Image Large",
                ModifiedDate = DateTime.UtcNow,
                Name = "Test Name",
                TargetGender = Gender.Female,
                Options = new List<PlanOptionViewModel>(),
                BannerColour = BannerColour.White,
                Active = true,
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.PlanMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                Plan toObject = new Plan();

                var mapper = new Map.PlanMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PlanViewModel webObject, Plan repoObject)
            {
                Assert.AreEqual(webObject.Active, repoObject.Active);
                Assert.AreEqual(webObject.BannerColour, repoObject.BannerColour);
                Assert.AreEqual(webObject.BannerHeader, repoObject.BannerHeader);
                Assert.AreEqual(webObject.CreatedDate, repoObject.CreatedDate);
                Assert.IsNotNull(webObject.ModifiedDate);
                Assert.AreEqual(webObject.Description, repoObject.Description);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.ImagePathLarge, repoObject.ImagePathLarge);
                Assert.AreEqual(webObject.ModifiedDate, repoObject.ModifiedDate);
                Assert.AreEqual(webObject.Name, repoObject.Name);
                Assert.AreEqual(webObject.TargetGender, repoObject.TargetGender);
                Assert.AreEqual(0, repoObject.Options.Count);
            }
        }
    }
}
