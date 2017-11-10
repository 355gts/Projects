using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class SelectedPlanOptionMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new PlanOption()
                {
                    Description = "4 Week Plan",
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                    Plan = new Plan()
                    {
                        Active = true,
                        BannerHeader = "Test Banner Header",
                        CreatedDate = DateTime.UtcNow,
                        Description = "Test Description",
                        Id = 123,
                        ImagePathLarge = "Test Image Path",
                        ImagePathMedium = "Test Image Path Medium",
                        ModifiedDate = DateTime.UtcNow,
                        Name = "Test Name",
                        TargetGender = Gender.Male,
                    },
                };

                var mapper = new Map.SelectedPlanOptionMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new PlanOption()
                {
                    Description = "4 Week Plan",
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                    Plan = new Plan()
                    {
                        Active = true,
                        BannerHeader = "Test Banner Header",
                        CreatedDate = DateTime.UtcNow,
                        Description = "Test Description",
                        Id = 123,
                        ImagePathLarge = "Test Image Path",
                        ImagePathMedium = "Test Image Path Medium",
                        ModifiedDate = DateTime.UtcNow,
                        Name = "Test Name",
                        TargetGender = Gender.Male,
                    },
                };

                SelectedPlanOptionViewModel toObject = new SelectedPlanOptionViewModel();

                var mapper = new Map.SelectedPlanOptionMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PlanOption repoObject, SelectedPlanOptionViewModel webObject)
            {
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.PlanId, webObject.PlanId);
                Assert.AreEqual(repoObject.Price, webObject.Price);

                // check plan is mapped
                Assert.AreEqual(repoObject.Plan.BannerHeader, webObject.Plan.BannerHeader);
                Assert.AreEqual(repoObject.Plan.CreatedDate, webObject.Plan.CreatedDate);
                Assert.AreEqual(repoObject.Plan.Description, webObject.Plan.Description);
                Assert.AreEqual(repoObject.Plan.Id, webObject.Plan.Id);
                Assert.AreEqual(repoObject.Plan.ImagePathLarge, webObject.Plan.ImagePathLarge);
                Assert.AreEqual(repoObject.Plan.ImagePathMedium, webObject.Plan.ImagePathMedium);
                Assert.AreEqual(repoObject.Plan.ModifiedDate, webObject.Plan.ModifiedDate);
                Assert.AreEqual(repoObject.Plan.Name, webObject.Plan.Name);
                Assert.AreEqual(repoObject.Plan.TargetGender, webObject.Plan.TargetGender);
            }
        }
    }
}
