using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class HallOfFameMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            CustomerPlan repoObject = new CustomerPlan()
            {
                AfterImage = "AfterImage",
                BeforeImage = "BeforeImage",
                Comment = "Comment",
                HallOfFameEnabled = true,
                Customer = new Customer()
                {
                    Firstname = "Firstname",
                    Surname = "Surname",
                },
                OrderId = 123,
                Item = new Item()
                {
                    Name = "ItemName",
                    Description = "ItemDescription",
                }
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.HallOfFameMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var toObject = new HallOfFameViewModel();

                var mapper = new Map.HallOfFameMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(CustomerPlan repoObject, HallOfFameViewModel webObject)
            {
                Assert.AreEqual(repoObject.AfterImage, webObject.AfterImagePath);
                Assert.AreEqual(repoObject.BeforeImage, webObject.BeforeImagePath);
                Assert.AreEqual(repoObject.Comment, webObject.Comment);
                Assert.AreEqual(repoObject.HallOfFameEnabled, webObject.Enabled);
                Assert.AreEqual($"{repoObject.Customer?.Firstname} {repoObject.Customer?.Surname}", webObject.Name);
                Assert.AreEqual(repoObject.OrderId, webObject.OrderId);
                Assert.AreEqual(repoObject.Item?.Name, webObject.PlanName);
                Assert.AreEqual(repoObject.Item?.Description, webObject.PlanDescription);
            }
        }
    }
}
