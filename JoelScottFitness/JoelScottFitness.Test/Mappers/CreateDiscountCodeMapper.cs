using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateDiscountCodeMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreateDiscountCodeViewModel()
                {
                    Code = "Code",
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var mapper = new Map.CreateDiscountCodeMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreateDiscountCodeViewModel()
                {
                    Code = "Code",
                    PercentDiscount = 12,
                    ValidFrom = new DateTime(2017, 05, 02, 14, 14, 15),
                    ValidTo = new DateTime(2037, 05, 23, 14, 14, 15),
                };

                var repoObject = new DiscountCode();

                var mapper = new Map.CreateDiscountCodeMapper();

                mapper.Map(webObject, repoObject);

                AssertAreEqual(webObject, repoObject);
            }

            private void AssertAreEqual(CreateDiscountCodeViewModel webObject, DiscountCode repoObject)
            {
                Assert.AreEqual(webObject.Code, repoObject.Code);
                Assert.AreEqual(webObject.PercentDiscount, repoObject.PercentDiscount);
                Assert.AreEqual(webObject.ValidFrom, repoObject.ValidFrom);
                Assert.AreEqual(webObject.ValidTo, repoObject.ValidTo);
            }
        }
    }
}
