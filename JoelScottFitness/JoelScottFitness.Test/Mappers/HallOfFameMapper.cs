using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class HallOfFameMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new PurchasedItem()
                {
                    Id = 123,
                    BeforeImage = "BeforeImage",
                    AfterImage = "AfterImage",
                    Comment = "Comment",
                    ItemId = 123,
                    Item = new Item()
                    {
                        Description = "Description",
                    },
                    Purchase = new Purchase()
                    {
                        Customer = new Customer()
                        {
                            Firstname = "Firstname",
                            Surname = "Surname",
                        }
                    }
                };

                var mapper = new Map.HallOfFameMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new PurchasedItem()
                {
                    Id = 123,
                    BeforeImage = "BeforeImage",
                    AfterImage = "AfterImage",
                    Comment = "Comment",
                    ItemId = 123,
                    Item = new Item()
                    {
                        Description = "Description",
                    },
                    Purchase = new Purchase()
                    {
                        Customer = new Customer()
                        {
                            Firstname = "Firstname",
                            Surname = "Surname",
                        }
                    }
                };

                var toObject = new HallOfFameViewModel();

                var mapper = new Map.HallOfFameMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(PurchasedItem repoObject, HallOfFameViewModel webObject)
            {
                Assert.AreEqual(repoObject.Id, webObject.PurchasedItemId);
                Assert.AreEqual(repoObject.AfterImage, webObject.AfterImagePath);
                Assert.AreEqual(repoObject.BeforeImage, webObject.BeforeImagePath);
                Assert.AreEqual(repoObject.BeforeImage, webObject.BeforeImagePath);
                Assert.AreEqual(repoObject.ItemId, webObject.ItemId);
                Assert.AreEqual($"{repoObject.Purchase.Customer.Firstname} {repoObject.Purchase.Customer.Surname}", webObject.Name);
                Assert.AreEqual(repoObject.Item.Description, webObject.PlanDescription);
            }
        }
    }
}
