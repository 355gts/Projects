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
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Plan()
                {
                    Active = true,
                    BannerHeader = "Test Banner",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Test Description",
                    Id = 123,
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    ModifiedDate = DateTime.UtcNow,
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>(),
                };

                var mapper = new Map.PlanMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Plan()
                {
                    Active = true,
                    BannerHeader = "Test Banner",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Test Description",
                    Id = 123,
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    ModifiedDate = DateTime.UtcNow,
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOption>(),
                };

                PlanViewModel toObject = new PlanViewModel();

                var mapper = new Map.PlanMapper();
                
                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Plan repoObject, PlanViewModel webObject)
            {
                Assert.AreEqual(repoObject.Active, webObject.Active);
                Assert.AreEqual(repoObject.BannerHeader, webObject.BannerHeader);
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePathLarge, webObject.ImagePathLarge);
                Assert.AreEqual(repoObject.ImagePathMedium, webObject.ImagePathMedium);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.TargetGender, webObject.TargetGender);
                Assert.AreEqual(0, webObject.Options.Count);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PlanViewModel()
                {
                    Active = true,
                    BannerHeader = "Test Banner",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Test Description",
                    Id = 123,
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    ModifiedDate = DateTime.UtcNow,
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOptionViewModel>(),
                };

                var mapper = new Map.PlanMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new PlanViewModel()
                {
                    Active = true,
                    BannerHeader = "Test Banner",
                    CreatedDate = DateTime.UtcNow,
                    Description = "Test Description",
                    Id = 123,
                    ImagePathLarge = "Image Large",
                    ImagePathMedium = "Image Medium",
                    ModifiedDate = DateTime.UtcNow,
                    Name = "Test Name",
                    TargetGender = Gender.Female,
                    Options = new List<PlanOptionViewModel>(),
                };

                Plan toObject = new Plan();

                var mapper = new Map.PlanMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PlanViewModel repoObject, Plan webObject)
            {
                Assert.AreEqual(repoObject.Active, webObject.Active);
                Assert.AreEqual(repoObject.BannerHeader, webObject.BannerHeader);
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePathLarge, webObject.ImagePathLarge);
                Assert.AreEqual(repoObject.ImagePathMedium, webObject.ImagePathMedium);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.TargetGender, webObject.TargetGender);
                Assert.AreEqual(0, webObject.Options.Count);
            }
        }
    }
}
