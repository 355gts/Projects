﻿using JoelScottFitness.Common.Enumerations;
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
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new PlanOption()
                {
                    Description = "4 Week Plan",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                };

                var mapper = new Map.PlanOptionMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new PlanOption()
                {
                    Description = "4 Week Plan",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                };
                
                PlanOptionViewModel toObject = new PlanOptionViewModel();

                var mapper = new Map.PlanOptionMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PlanOption repoObject, PlanOptionViewModel webObject)
            {
                Assert.AreEqual(repoObject.ActiveFrom, webObject.ActiveFrom);
                Assert.AreEqual(repoObject.ActiveTo, webObject.ActiveTo);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.PlanId, webObject.PlanId);
                Assert.AreEqual(repoObject.Price, webObject.Price);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PlanOptionViewModel()
                {
                    Description = "4 Week Plan",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                };

                var mapper = new Map.PlanOptionMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new PlanOptionViewModel()
                {
                    Description = "4 Week Plan",
                    ActiveFrom = DateTime.UtcNow,
                    ActiveTo = DateTime.UtcNow.AddYears(1),
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                    PlanId = 12,
                    Id = 34,
                };

                PlanOption toObject = new PlanOption();

                var mapper = new Map.PlanOptionMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PlanOptionViewModel webObject, PlanOption repoObject)
            {
                Assert.AreEqual(repoObject.ActiveFrom, webObject.ActiveFrom);
                Assert.AreEqual(repoObject.ActiveTo, webObject.ActiveTo);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.PlanId, webObject.PlanId);
                Assert.AreEqual(repoObject.Price, webObject.Price);
            }
        }
    }
}
