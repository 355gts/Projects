using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class OrderItemMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            OrderItem repoObject = new OrderItem()
            {
                Id = 123,
                ItemId = 456,
                Item = new Item(),
                Quantity = 2,
                ItemCategory = ItemCategory.Plan,
                Price = 12.34,
                ItemDiscounted = true,
                OrderId = 123,
                RequiresAction = true,
                Total = 12.34,
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.OrderItemMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                OrderItemViewModel toObject = new OrderItemViewModel();

                var mapper = new Map.OrderItemMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(OrderItem repoObject, OrderItemViewModel webObject)
            {
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ItemCategory, webObject.ItemCategory);
                Assert.AreEqual(repoObject.ItemDiscounted, webObject.ItemDiscounted);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual(repoObject.OrderId, webObject.OrderId);
                Assert.AreEqual(repoObject.Price, webObject.Price);
                Assert.AreEqual(repoObject.Quantity, webObject.Quantity);
                Assert.AreEqual(repoObject.RequiresAction, webObject.RequiresAction);
                Assert.AreEqual(repoObject.Total, webObject.ItemTotal);
                Assert.IsNotNull(webObject.Item);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            OrderItemViewModel webObject = new OrderItemViewModel()
            {
                Id = 123,
                ItemId = 456,
                ItemCategory = ItemCategory.Plan,
                Price = 12.34,
                Quantity = 2,
                RequiresAction = true,
                OrderId = 123,
            };

            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var mapper = new Map.OrderItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                OrderItem toObject = new OrderItem();

                var mapper = new Map.OrderItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(OrderItemViewModel webObject, OrderItem repoObject)
            {
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.ItemId, repoObject.ItemId);
                Assert.AreEqual(webObject.Quantity, repoObject.Quantity);
                Assert.AreEqual(webObject.RequiresAction, repoObject.RequiresAction);
                Assert.AreEqual(webObject.ItemTotal, webObject.Total);
            }
        }
    }
}
