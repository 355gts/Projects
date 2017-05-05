using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class AddressMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Address()
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLins3",
                    City = "City",
                    Id = 456,
                    PostCode = "PostCode",
                    Region = "Region",
                    Country = "Country",
                    CountryCode = "CountryCode",
                };

                var mapper = new Map.AddressMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Address()
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLins3",
                    City = "City",
                    Id = 456,
                    PostCode = "PostCode",
                    Region = "Region",
                    Country = "Country",
                    CountryCode = "CountryCode",
                };

                AddressViewModel toObject = new AddressViewModel();

                var mapper = new Map.AddressMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Address repoObject, AddressViewModel webObject)
            {
                Assert.AreEqual(repoObject.AddressLine1, webObject.AddressLine1);
                Assert.AreEqual(repoObject.AddressLine2, webObject.AddressLine2);
                Assert.AreEqual(repoObject.AddressLine3, webObject.AddressLine3);
                Assert.AreEqual(repoObject.City, webObject.City);
                Assert.AreEqual(repoObject.Country, webObject.Country);
                Assert.AreEqual(repoObject.CountryCode, webObject.CountryCode);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.PostCode, webObject.PostCode);
                Assert.AreEqual(repoObject.Region, webObject.Region);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new AddressViewModel()
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLins3",
                    City = "City",
                    Id = 456,
                    PostCode = "PostCode",
                    Region = "Region",
                    Country = "Country",
                    CountryCode = "CountryCode",
                };

                var mapper = new Map.AddressMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new AddressViewModel()
                {
                    AddressLine1 = "AddressLine1",
                    AddressLine2 = "AddressLine2",
                    AddressLine3 = "AddressLins3",
                    City = "City",
                    Id = 456,
                    PostCode = "PostCode",
                    Region = "Region",
                    Country = "Country",
                    CountryCode = "CountryCode",
                };

                Address toObject = new Address();

                var mapper = new Map.AddressMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(AddressViewModel webObject, Address repoObject)
            {
                Assert.AreEqual(webObject.AddressLine1, repoObject.AddressLine1);
                Assert.AreEqual(webObject.AddressLine2, repoObject.AddressLine2);
                Assert.AreEqual(webObject.AddressLine3, repoObject.AddressLine3);
                Assert.AreEqual(webObject.City, repoObject.City);
                Assert.AreEqual(webObject.Country, repoObject.Country);
                Assert.AreEqual(webObject.CountryCode, repoObject.CountryCode);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.PostCode, repoObject.PostCode);
                Assert.AreEqual(webObject.Region, repoObject.Region);
            }
        }
    }
}
