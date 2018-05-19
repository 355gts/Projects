using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CustomerPlanMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            CustomerPlan repoObject = new CustomerPlan()
            {
                CustomerId = Guid.NewGuid(),
                Id = 123,
                ItemId = 456,
                Item = new Item()
                {
                    Name = "Name",
                    ImagePath = "ImagePath",
                },
                MemberOfHallOfFame = true,
                OrderId = 678,
                PlanPath = "PlantPath",
                QuestionnaireComplete = true,
                SheetsUri = "SheetsUri",
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.CustomerPlanMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CustomerPlanViewModel();

                var mapper = new Map.CustomerPlanMapper();

                mapper.Map(repoObject, webObject);

                AssertAreEqual(repoObject, webObject);
            }

            private void AssertAreEqual(CustomerPlan repoObject, CustomerPlanViewModel webObject)
            {
                Assert.AreEqual(repoObject.CustomerId, webObject.CustomerId);
                Assert.AreEqual(repoObject.Item.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.Item?.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.Item.Name, webObject.Name);
                Assert.AreEqual(repoObject.OrderId, webObject.OrderId);
                Assert.AreEqual(repoObject.PlanPath, webObject.PlanPath);
                Assert.AreEqual(repoObject.QuestionnaireComplete, webObject.QuestionnaireComplete);
                Assert.AreEqual(repoObject.RequiresAction, webObject.RequiresAction);
                Assert.AreEqual(repoObject.SheetsUri, webObject.SheetsUri);
            }
        }
    }
}
