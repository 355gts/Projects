using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class AddOrderItemMapper
    {
        [TestClass]
        public class WebToRepo
        {
            BasketItemViewModel webObject = new BasketItemViewModel()
            {
                Id = 123,
                ItemCategory = ItemCategory.Plan,
                Price = 10.99,
                Quantity = 2,
                ItemDiscounted = true,
                DiscountPercent = 10,
            };

            [TestMethod]
            public void FromObject_ToNullObject_Discounted()
            {
                var mapper = new Map.AddOrderItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);

                Assert.IsTrue(result.ItemDiscounted);
                Assert.AreEqual(webObject.DiscountedPrice, result.Price);
                Assert.AreEqual(webObject.ItemTotal, result.Total);
            }

            [TestMethod]
            public void FromObject_ToObject_Discounted()
            {
                OrderItem toObject = new OrderItem();

                var mapper = new Map.AddOrderItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);

                Assert.IsTrue(toObject.ItemDiscounted);
                Assert.AreEqual(webObject.DiscountedPrice, toObject.Price);
                Assert.AreEqual(webObject.ItemTotal, toObject.Total);
            }

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                webObject.ItemDiscounted = false;

                var mapper = new Map.AddOrderItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);

                Assert.IsFalse(result.ItemDiscounted);
                Assert.AreEqual(webObject.Price, result.Price);
                Assert.AreEqual(Math.Round(webObject.ItemTotal, 2), result.Total);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                webObject.ItemDiscounted = false;

                OrderItem toObject = new OrderItem();

                var mapper = new Map.AddOrderItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);

                Assert.IsFalse(toObject.ItemDiscounted);
                Assert.AreEqual(webObject.Price, toObject.Price);
                Assert.AreEqual(Math.Round(webObject.ItemTotal, 2), toObject.Total);
            }

            private void AssertAreEqual(BasketItemViewModel webObject, OrderItem repoObject)
            {
                Assert.AreEqual(webObject.Id, repoObject.ItemId);
                Assert.AreEqual(webObject.Quantity, repoObject.Quantity);
                Assert.AreEqual(webObject.ItemCategory, repoObject.ItemCategory);
            }
        }
    }
}
