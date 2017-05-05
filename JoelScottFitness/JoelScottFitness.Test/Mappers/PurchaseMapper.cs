using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class PurchaseMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Purchase()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    SalesReference = "SalesReference",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItem>()
                    {
                        new PurchasedItem()
                        {
                            Description = "Description",
                            Id = 123,
                            ItemId = 456,
                            ItemType = ItemType.Plan,
                            Price = 12.34,
                        }
                    }
                };

                var mapper = new Map.PurchaseMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Purchase()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    SalesReference = "SalesReference",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItem>()
                    {
                        new PurchasedItem()
                        {
                            Description = "Description",
                            Id = 123,
                            ItemId = 456,
                            ItemType = ItemType.Plan,
                            Price = 12.34,
                        }
                    }
                };

                PurchaseViewModel toObject = new PurchaseViewModel();

                var mapper = new Map.PurchaseMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Purchase repoObject, PurchaseViewModel webObject)
            {
                Assert.AreEqual(repoObject.CustomerId, webObject.CustomerId);
                Assert.AreEqual(repoObject.DiscountCodeId, webObject.DiscountCodeId);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PayPalReference, webObject.PayPalReference);
                Assert.AreEqual(repoObject.PurchaseDate, webObject.PurchaseDate);
                Assert.AreEqual(repoObject.SalesReference, webObject.SalesReference);
                Assert.AreEqual(repoObject.TotalAmount, webObject.TotalAmount);

                Assert.IsNotNull(webObject.Items);

                var repoItem = repoObject.Items.First();
                var webItem = webObject.Items.First();

                Assert.AreEqual(repoItem.Description, webItem.Description);
                Assert.AreEqual(repoItem.Id, webItem.Id);
                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.ItemType, webItem.ItemType);
                Assert.AreEqual(repoItem.Price, webItem.Price);

            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PurchaseViewModel()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    SalesReference = "SalesReference",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItemViewModel>()
                    {
                        new PurchasedItemViewModel()
                        {
                            Description = "Description",
                            Id = 123,
                            ItemId = 456,
                            ItemType = ItemType.Plan,
                            Price = 12.34,
                        }
                    }
                };

                var mapper = new Map.PurchaseMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new PurchaseViewModel()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    SalesReference = "SalesReference",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItemViewModel>()
                    {
                        new PurchasedItemViewModel()
                        {
                            Description = "Description",
                            Id = 123,
                            ItemId = 456,
                            ItemType = ItemType.Plan,
                            Price = 12.34,
                        }
                    }
                };

                Purchase toObject = new Purchase();

                var mapper = new Map.PurchaseMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(PurchaseViewModel repoObject, Purchase webObject)
            {
                Assert.AreEqual(webObject.CustomerId, repoObject.CustomerId);
                Assert.AreEqual(webObject.DiscountCodeId, repoObject.DiscountCodeId);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.PayPalReference, repoObject.PayPalReference);
                Assert.AreEqual(webObject.PurchaseDate, repoObject.PurchaseDate);
                Assert.AreEqual(webObject.SalesReference, repoObject.SalesReference);
                Assert.AreEqual(webObject.TotalAmount, repoObject.TotalAmount);

                Assert.IsNotNull(repoObject.Items);

                var repoItem = webObject.Items.First();
                var webItem = repoObject.Items.First();

                Assert.AreEqual(repoItem.Description, webItem.Description);
                Assert.AreEqual(repoItem.Id, webItem.Id);
                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.ItemType, webItem.ItemType);
                Assert.AreEqual(repoItem.Price, webItem.Price);

            }
        }
    }
}
