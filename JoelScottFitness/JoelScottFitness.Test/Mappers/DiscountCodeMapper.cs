using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class DiscountCodeMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new DiscountCode()
                {
                    Code = "Code",
                    Id = 111,
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var mapper = new Map.DiscountCodeMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new DiscountCode()
                {
                    Code = "Code",
                    Id = 111,
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var webObject = new DiscountCodeViewModel();

                var mapper = new Map.DiscountCodeMapper();

                mapper.Map(repoObject, webObject);

                AssertAreEqual(repoObject, webObject);
            }

            private void AssertAreEqual(DiscountCode repoObject, DiscountCodeViewModel webObject)
            {
                Assert.IsTrue(webObject.Active);
                Assert.AreEqual(repoObject.Code, webObject.Code);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PercentDiscount, webObject.PercentDiscount);
                Assert.AreEqual(repoObject.ValidFrom, webObject.ValidFrom);
                Assert.AreEqual(repoObject.ValidTo, webObject.ValidTo);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new DiscountCodeViewModel()
                {
                    Code = "Code",
                    Id = 111,
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var mapper = new Map.DiscountCodeMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new DiscountCodeViewModel()
                {
                    Code = "Code",
                    Id = 111,
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var repoObject = new DiscountCode();

                var mapper = new Map.DiscountCodeMapper();

                mapper.Map(webObject, repoObject);

                AssertAreEqual(webObject, repoObject);
            }

            private void AssertAreEqual(DiscountCodeViewModel webObject, DiscountCode repoObject)
            {
                Assert.AreEqual(webObject.Code, repoObject.Code);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.PercentDiscount, repoObject.PercentDiscount);
                Assert.AreEqual(webObject.ValidFrom, repoObject.ValidFrom);
                Assert.AreEqual(webObject.ValidTo, repoObject.ValidTo);
            }
        }
    }
}
