using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class PlanOptionMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            PlanOption repoObject = new PlanOption()
            {
                Description = "4 Week Plan",
                Duration = 4,
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                PlanId = 12,
                Id = 34,
                ImagePath = "ImagePath",
                CreatedDate = new DateTime(2018, 09, 04, 12, 34, 40),
                ItemDiscontinued = true,
                ModifiedDate = new DateTime(2018, 10, 04, 12, 34, 40),
                Name = "Name",
                Plan = new Plan()
                //{
                //    Active = true,
                //    BannerHeader = "Test Banner Header",
                //    CreatedDate = new DateTime(2018, 09, 13, 12, 34, 40),
                //    Description = "Test Description",
                //    Id = 123,
                //    ImagePathLarge = "Test Image Path",
                //    ModifiedDate = new DateTime(2018, 09, 05, 12, 34, 40),
                //    Name = "Test Name",
                //    TargetGender = Gender.Male,
                //    BannerColour = BannerColour.Black,
                //},
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.PlanOptionMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                PlanOptionViewModel toObject = new PlanOptionViewModel();

                var mapper = new Map.PlanOptionMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PlanOption repoObject, PlanOptionViewModel webObject)
            {
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.PlanId, webObject.PlanId);
                Assert.AreEqual(repoObject.Price, webObject.Price);

                // check plan is mapped
                //Assert.AreEqual(repoObject.Plan.Active, webObject.Plan.Active);
                //Assert.AreEqual(repoObject.Plan.BannerColour, webObject.Plan.BannerColour);
                //Assert.AreEqual(repoObject.Plan.BannerHeader, webObject.Plan.BannerHeader);
                //Assert.AreEqual(repoObject.Plan.CreatedDate, webObject.Plan.CreatedDate);
                //Assert.AreEqual(repoObject.Plan.Description, webObject.Plan.Description);
                //Assert.AreEqual(repoObject.Plan.Id, webObject.Plan.Id);
                //Assert.AreEqual(repoObject.Plan.ImagePathLarge, webObject.Plan.ImagePathLarge);
                //Assert.AreEqual(repoObject.Plan.ModifiedDate, webObject.Plan.ModifiedDate);
                //Assert.AreEqual(repoObject.Plan.Name, webObject.Plan.Name);
                //Assert.AreEqual(repoObject.Plan.TargetGender, webObject.Plan.TargetGender);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            PlanOptionViewModel webObject = new PlanOptionViewModel()
            {
                Name = "Name",
                CreatedDate = new DateTime(2018, 09, 04, 12, 34, 40),
                ItemDiscontinued = true,
                Description = "4 Week Plan",
                Duration = 4,
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                PlanId = 12,
                Id = 34,
                ImagePath = "ImagePath",
                Plan = new PlanViewModel()
                //{
                //    BannerHeader = "Test Banner Header",
                //    CreatedDate = new DateTime(2018, 09, 04, 12, 34, 40),
                //    Description = "Test Description",
                //    Id = 123,
                //    ImagePathLarge = "Test Image Path",
                //    ModifiedDate = new DateTime(2018, 09, 05, 12, 34, 40),
                //    Name = "Test Name",
                //    TargetGender = Gender.Male,
                //},
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.PlanOptionMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                PlanOption toObject = new PlanOption();

                var mapper = new Map.PlanOptionMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PlanOptionViewModel webObject, PlanOption repoObject)
            {
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.PlanId, webObject.PlanId);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.IsNotNull(repoObject.ModifiedDate);

                // check plan is mapped
                //Assert.AreEqual(repoObject.Plan.Active, webObject.Plan.Active);
                //Assert.AreEqual(repoObject.Plan.BannerColour, webObject.Plan.BannerColour);
                //Assert.AreEqual(repoObject.Plan.BannerHeader, webObject.Plan.BannerHeader);
                //Assert.AreEqual(repoObject.Plan.CreatedDate, webObject.Plan.CreatedDate);
                //Assert.AreEqual(repoObject.Plan.Description, webObject.Plan.Description);
                //Assert.AreEqual(repoObject.Plan.Id, webObject.Plan.Id);
                //Assert.AreEqual(repoObject.Plan.ImagePathLarge, webObject.Plan.ImagePathLarge);
                //Assert.AreEqual(repoObject.Plan.ModifiedDate, webObject.Plan.ModifiedDate);
                //Assert.AreEqual(repoObject.Plan.Name, webObject.Plan.Name);
                //Assert.AreEqual(repoObject.Plan.TargetGender, webObject.Plan.TargetGender);
            }
        }
    }
}
