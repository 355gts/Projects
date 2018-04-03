using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateItemMapper
    {
        [TestClass]
        public class WebToRepo
        {
            CreateItemViewModel webObject = new CreateItemViewModel()
            {
                Description = "Description",
                ImagePath = "ImagePath",
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                Name = "Name",
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.CreateItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var toObject = new Item();

                var mapper = new Map.CreateItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreateItemViewModel webObject, Item repoObject)
            {
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.IsFalse(repoObject.ItemDiscontinued);
                Assert.IsNotNull(repoObject.CreatedDate);
                Assert.IsNull(repoObject.ModifiedDate);
            }
        }
    }
}
