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
            CreatePlanOptionViewModel webObject = new CreatePlanOptionViewModel()
            {
                Description = "Description",
                Duration = 4,
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                Name = "Name"
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.CreatePlanOptionMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var toObject = new PlanOption();

                var mapper = new Map.CreatePlanOptionMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreatePlanOptionViewModel webObject, PlanOption repoObject)
            {
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Duration, webObject.Duration);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.IsFalse(repoObject.ItemDiscontinued);
            }
        }
    }
}
