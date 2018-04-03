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
    public class OrderMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Order()
                {
                    CustomerId = Guid.NewGuid(),
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    TransactionId = "SalesReference",
                    TotalAmount = 1234,
                    QuestionnareId = 333,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            ItemId = 456,
                            Quantity= 23,
                            //Description = "Description",
                            ItemCategory = ItemCategory.Plan,
                            Price = 2.34,
                            //Name = "Name",
                        }
                    }
                };

                var mapper = new Map.OrderMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Order()
                {
                    CustomerId = Guid.NewGuid(),
                    DiscountCodeId = 456,
                    Id = 789,
                    PayPalReference = "PayPalReference",
                    PurchaseDate = DateTime.UtcNow,
                    TransactionId = "TransactionId",
                    TotalAmount = 1234,
                    QuestionnareId = 333,
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            ItemId = 456,
                            Quantity = 23,
                            //Description = "Description",
                            ItemCategory = ItemCategory.Plan,
                            Price = 2.34,
                            //Name = "Name",
                        }
                    }
                };

                PurchaseHistoryViewModel toObject = new PurchaseHistoryViewModel();

                var mapper = new Map.OrderMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Order repoObject, PurchaseHistoryViewModel webObject)
            {
                Assert.AreEqual(repoObject.CustomerId, webObject.CustomerId);
                Assert.AreEqual(repoObject.DiscountCodeId, webObject.DiscountCodeId);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PayPalReference, webObject.PayPalReference);
                Assert.AreEqual(repoObject.PurchaseDate, webObject.PurchaseDate);
                Assert.AreEqual(repoObject.TransactionId, webObject.TransactionId);
                Assert.AreEqual(repoObject.TotalAmount, webObject.TotalAmount);
                Assert.AreEqual(repoObject.QuestionnareId, webObject.QuestionnaireId);

                Assert.IsNotNull(webObject.Items);

                var repoItem = repoObject.Items.First();
                var webItem = webObject.Items.First();

                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.Quantity, webItem.Quantity);
            }
        }
    }
}
