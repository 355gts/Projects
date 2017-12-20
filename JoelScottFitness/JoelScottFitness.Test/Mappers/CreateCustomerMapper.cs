using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateCustomerMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreateCustomerViewModel()
                {
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Surname = "Surname",
                    BillingAddress = new AddressViewModel()
                    {
                        AddressLine1 = "AddressLine1",
                        AddressLine2 = "AddressLine2",
                        AddressLine3 = "AddressLine3",
                        City = "City",
                        Country = "Country",
                        CountryCode = "CountryCode",
                        PostCode = "PostCode",
                        Id = 123,
                        Region = "Region",
                    },
                };

                var mapper = new Map.CreateCustomerMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreateCustomerViewModel()
                {
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Surname = "Surname",
                    BillingAddress = new AddressViewModel()
                    {
                        AddressLine1 = "AddressLine1",
                        AddressLine2 = "AddressLine2",
                        AddressLine3 = "AddressLine3",
                        City = "City",
                        Country = "Country",
                        CountryCode = "CountryCode",
                        PostCode = "PostCode",
                        Id = 123,
                        Region = "Region",
                    },
                };

                Customer toObject = new Customer();

                var mapper = new Map.CreateCustomerMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreateCustomerViewModel repoObject, Customer webObject)
            {
                Assert.AreEqual(webObject.EmailAddress, repoObject.EmailAddress);
                Assert.AreEqual(webObject.Firstname, repoObject.Firstname);
                Assert.AreEqual(webObject.Surname, repoObject.Surname);

                Assert.AreEqual(webObject.BillingAddress.AddressLine1, repoObject.BillingAddress.AddressLine1);
                Assert.AreEqual(webObject.BillingAddress.AddressLine2, repoObject.BillingAddress.AddressLine2);
                Assert.AreEqual(webObject.BillingAddress.AddressLine3, repoObject.BillingAddress.AddressLine3);
                Assert.AreEqual(webObject.BillingAddress.City, repoObject.BillingAddress.City);
                Assert.AreEqual(webObject.BillingAddress.Country, repoObject.BillingAddress.Country);
                Assert.AreEqual(webObject.BillingAddress.CountryCode, repoObject.BillingAddress.CountryCode);
                Assert.AreEqual(webObject.BillingAddress.Id, repoObject.BillingAddress.Id);
                Assert.AreEqual(webObject.BillingAddress.PostCode, repoObject.BillingAddress.PostCode);
                Assert.AreEqual(webObject.BillingAddress.Region, repoObject.BillingAddress.Region);
            }
        }
    }
}
