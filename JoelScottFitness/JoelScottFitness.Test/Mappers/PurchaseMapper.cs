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
                    TransactionId = "SalesReference",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItem>()
                    {
                        new PurchasedItem()
                        {
                            ItemId = 456,
                            Quantity= 23
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
                    TransactionId = "TransactionId",
                    TotalAmount = 1234,
                    Items = new List<PurchasedItem>()
                    {
                        new PurchasedItem()
                        {
                            ItemId = 456,
                            Quantity = 23,
                        }
                    }
                };

                PurchaseHistoryViewModel toObject = new PurchaseHistoryViewModel();

                var mapper = new Map.PurchaseMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Purchase repoObject, PurchaseHistoryViewModel webObject)
            {
                Assert.AreEqual(repoObject.CustomerId, webObject.CustomerId);
                Assert.AreEqual(repoObject.DiscountCodeId, webObject.DiscountCodeId);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PayPalReference, webObject.PayPalReference);
                Assert.AreEqual(repoObject.PurchaseDate, webObject.PurchaseDate);
                Assert.AreEqual(repoObject.TransactionId, webObject.TransactionId);
                Assert.AreEqual(repoObject.TotalAmount, webObject.TotalAmount);

                Assert.IsNotNull(webObject.Items);

                var repoItem = repoObject.Items.First();
                var webItem = webObject.Items.First();
                
                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.Quantity, webItem.Quantity);

            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new PurchaseHistoryViewModel()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    TransactionId = "TransactionId",
                    TotalAmount = 1234,
                    Items = new List<PurchasedHistoryItemViewModel>()
                    {
                        new PurchasedHistoryItemViewModel()
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
                var webObject = new PurchaseHistoryViewModel()
                {
                    CustomerId = 123,
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    TransactionId = "TransactionId",
                    TotalAmount = 1234,
                    Items = new List<PurchasedHistoryItemViewModel>()
                    {
                        new PurchasedHistoryItemViewModel()
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

            private void AssertAreEqual(PurchaseHistoryViewModel repoObject, Purchase webObject)
            {
                Assert.AreEqual(webObject.CustomerId, repoObject.CustomerId);
                Assert.AreEqual(webObject.DiscountCodeId, repoObject.DiscountCodeId);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.PayPalReference, repoObject.PayPalReference);
                Assert.AreEqual(webObject.PurchaseDate, repoObject.PurchaseDate);
                Assert.AreEqual(webObject.TransactionId, repoObject.TransactionId);
                Assert.AreEqual(webObject.TotalAmount, repoObject.TotalAmount);

                Assert.IsNotNull(repoObject.Items);

                var repoItem = webObject.Items.First();
                var webItem = repoObject.Items.First();
                
                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.Quantity, webItem.Quantity);

            }
        }
    }
}
