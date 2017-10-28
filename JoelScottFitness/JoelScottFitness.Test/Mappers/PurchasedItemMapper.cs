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
                    ItemId = 456,
                    Quantity = 2,
                    Item = new Item()
                    {
                        ItemType = ItemType.Plan,
                        Price = 12.34,
                        Description = "Description",
                        Id = 123,
                    }
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
                    ItemId = 456,
                    Quantity = 2,
                    Item = new Item()
                    {
                        ItemType = ItemType.Plan,
                        Price = 12.34,
                        Description = "Description",
                        Id = 123,
                    }
                };

                PurchasedHistoryItemViewModel toObject = new PurchasedHistoryItemViewModel();

                var mapper = new Map.PurchasedItemMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PurchasedItem repoObject, PurchasedHistoryItemViewModel webObject)
            {
                Assert.AreEqual(repoObject.Item.Description, webObject.Description);
                Assert.AreEqual(repoObject.Item.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.Item.ItemType, webObject.ItemType);
                Assert.AreEqual(repoObject.Item.Price, webObject.Price);
                Assert.AreEqual(repoObject.Quantity, webObject.Quantity);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PurchasedHistoryItemViewModel()
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
                var webObject = new PurchasedHistoryItemViewModel()
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

            private void AssertAreEqual(PurchasedHistoryItemViewModel repoObject, PurchasedItem webObject)
            {
                Assert.AreEqual(repoObject.Description, webObject.Item.Description);
                Assert.AreEqual(repoObject.Id, webObject.Item.Id);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.ItemType, webObject.Item.ItemType);
                Assert.AreEqual(repoObject.Price, webObject.Item.Price);
                Assert.AreEqual(repoObject.Quantity, webObject.Quantity);

            }
        }
    }
}
