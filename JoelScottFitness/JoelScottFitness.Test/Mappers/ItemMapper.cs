using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class ItemnMapper
    {
        [TestClass]
        public class WebToRepo
        {
            ItemViewModel webObject = new ItemViewModel()
            {
                Description = "Description",
                Id = 123,
                ImagePath = "ImagePath",
                ItemDiscontinued = true,
                CreatedDate = new DateTime(2018, 09, 03, 12, 34, 40),
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                Name = "Name",
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.ItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var toObject = new Item();

                var mapper = new Map.ItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(ItemViewModel webObject, Item repoObject)
            {
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.IsNotNull(repoObject.ModifiedDate);
            }
        }


        [TestClass]
        public class RepoToWeb
        {
            Item webObject = new Item()
            {
                Description = "Description",
                Id = 123,
                ImagePath = "ImagePath",
                ItemDiscontinued = true,
                CreatedDate = new DateTime(2018, 09, 03, 12, 34, 40),
                ItemCategory = ItemCategory.Plan,
                Price = 20.99,
                Name = "Name",
                ModifiedDate = new DateTime(2018, 09, 04, 12, 34, 40),
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.ItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var toObject = new ItemViewModel();

                var mapper = new Map.ItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(Item repoObject, ItemViewModel webObject)
            {
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.ItemDiscontinued, webObject.ItemDiscontinued);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
            }
        }
    }
}
