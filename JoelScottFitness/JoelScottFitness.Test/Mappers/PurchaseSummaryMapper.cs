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
    public class PurchaseSummaryMapper
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
                            Quantity= 23,
                            Item = new Item()
                            {
                                Description = "Description",
                                ItemType = ItemType.Plan,
                                Price = 2.34,
                            },
                        }
                    },
                    Customer = new Customer()
                    {
                        Firstname = "Firstname",
                        Surname = "Surname",
                    },
                    DiscountCode = new DiscountCode()
                    {
                        Code = "Code",
                    },
                    Questionnaire = new Questionnaire()
                };

                var mapper = new Map.PurchaseSummaryMapper();

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
                            Item = new Item()
                            {
                                Description = "Description",
                                ItemType = ItemType.Plan,
                                Price = 2.34,
                            },
                        }
                    },
                    Customer = new Customer()
                    {
                        Firstname = "Firstname",
                        Surname = "Surname",
                    },
                    DiscountCode = new DiscountCode()
                    {
                        Code = "Code",
                    },
                    Questionnaire = new Questionnaire()
                };

                PurchaseSummaryViewModel toObject = new PurchaseSummaryViewModel();

                var mapper = new Map.PurchaseSummaryMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Purchase repoObject, PurchaseSummaryViewModel webObject)
            {
                Assert.AreEqual(repoObject.CustomerId, webObject.CustomerId);
                Assert.AreEqual($"{repoObject.Customer.Firstname} {repoObject.Customer.Surname}", webObject.CustomerName);
                Assert.AreEqual(repoObject.DiscountCodeId, webObject.DiscountCodeId);
                Assert.AreEqual(repoObject.DiscountCode.Code, webObject.DiscountCode);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PayPalReference, webObject.PayPalReference);
                Assert.AreEqual(repoObject.PurchaseDate, webObject.PurchaseDate);
                Assert.AreEqual(repoObject.TransactionId, webObject.TransactionId);
                Assert.AreEqual(repoObject.TotalAmount, webObject.TotalAmount);
                Assert.IsTrue(webObject.QuestionnaireComplete);

                Assert.IsNotNull(webObject.Items);

                var repoItem = repoObject.Items.First();
                var webItem = webObject.Items.First();

                Assert.AreEqual(repoItem.ItemId, webItem.ItemId);
                Assert.AreEqual(repoItem.Quantity, webItem.Quantity);
            }
        }
    }
}
