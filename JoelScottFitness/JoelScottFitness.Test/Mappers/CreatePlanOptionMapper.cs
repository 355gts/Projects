using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreatePlanOptionMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreatePlanOptionViewModel()
                {
                    Description = "4 Week Plan",
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                };

                var mapper = new Map.CreatePlanOptionMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreatePlanOptionViewModel()
                {
                    Description = "4 Week Plan",
                    Duration = 4,
                    ItemType = ItemType.Plan,
                    Price = 20.99,
                };

                PlanOption toObject = new PlanOption();

                var mapper = new Map.CreatePlanOptionMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreatePlanOptionViewModel webObject, PlanOption repoObject)
            {
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.Price, webObject.Price);
            }
        }
    }
}
