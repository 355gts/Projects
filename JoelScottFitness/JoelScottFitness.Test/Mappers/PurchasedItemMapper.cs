using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class PurchasedItemMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new PurchasedItem()
                {
                    Description = "Description",
                    Id = 123,
                    ItemId = 456,
                    ItemType = ItemType.Plan,
                    Price = 12.34,
                    Quantity = 2,
                };

                var mapper = new Map.PurchasedItemMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new PurchasedItem()
                {
                    Description = "Description",
                    Id = 123,
                    ItemId = 456,
                    ItemType = ItemType.Plan,
                    Price = 12.34,
                    Quantity = 2,
                };

                PurchasedItemViewModel toObject = new PurchasedItemViewModel();

                var mapper = new Map.PurchasedItemMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PurchasedItem repoObject, PurchasedItemViewModel webObject)
            {
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.AreEqual(repoObject.Quantity, webObject.Quantity);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PurchasedItemViewModel()
                {
                    Description = "Description",
                    Id = 123,
                    ItemId = 456,
                    ItemType = ItemType.Plan,
                    Price = 12.34,
                    Quantity = 2,
                };

                var mapper = new Map.PurchasedItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new PurchasedItemViewModel()
                {
                    Description = "Description",
                    Id = 123,
                    ItemId = 456,
                    ItemType = ItemType.Plan,
                    Price = 12.34,
                    Quantity = 2,
                };

                PurchasedItem toObject = new PurchasedItem();

                var mapper = new Map.PurchasedItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PurchasedItemViewModel repoObject, PurchasedItem webObject)
            {
                Assert.AreEqual(repoObject.Description, webObject.Description);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.AreEqual(repoObject.Quantity, webObject.Quantity);

            }
        }
    }
}
