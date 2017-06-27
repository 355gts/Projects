using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using JoelScottFitness.Identity.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CustomerMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Customer()
                {
                    BillingAddressId = 123,
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Id = 456,
                    ModifiedDate = DateTime.UtcNow.AddMinutes(1),
                    Surname = "Surname",
                    UserId = 789,
                    BillingAddress = new Address()
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
                    PurchaseHistory = new List<Purchase>(),
                    User = new AuthUser()
                    {
                        Id = 123,
                        Email = "Email",
                        UserName = "Username",
                    }
                };

                var mapper = new Map.CustomerMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Customer()
                {
                    BillingAddressId = 123,
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Id = 456,
                    ModifiedDate = DateTime.UtcNow.AddMinutes(1),
                    Surname = "Surname",
                    UserId = 789,
                    BillingAddress = new Address()
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
                    PurchaseHistory = new List<Purchase>(),
                    User = new AuthUser()
                    {
                        Id = 123,
                        Email = "Email",
                        UserName = "Username",
                    }
                };

                CustomerViewModel toObject = new CustomerViewModel();

                var mapper = new Map.CustomerMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Customer repoObject, CustomerViewModel webObject)
            {
                Assert.AreEqual(repoObject.BillingAddressId, webObject.BillingAddressId);
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.EmailAddress, webObject.EmailAddress);
                Assert.AreEqual(repoObject.Firstname, webObject.Firstname);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
                Assert.AreEqual(repoObject.Surname, webObject.Surname);
                Assert.AreEqual(repoObject.UserId, webObject.UserId);
                
                Assert.AreEqual(repoObject.BillingAddress.AddressLine1, webObject.BillingAddress.AddressLine1);
                Assert.AreEqual(repoObject.BillingAddress.AddressLine2, webObject.BillingAddress.AddressLine2);
                Assert.AreEqual(repoObject.BillingAddress.AddressLine3, webObject.BillingAddress.AddressLine3);
                Assert.AreEqual(repoObject.BillingAddress.City, webObject.BillingAddress.City);
                Assert.AreEqual(repoObject.BillingAddress.Country, webObject.BillingAddress.Country);
                Assert.AreEqual(repoObject.BillingAddress.CountryCode, webObject.BillingAddress.CountryCode);
                Assert.AreEqual(repoObject.BillingAddress.Id, webObject.BillingAddress.Id);
                Assert.AreEqual(repoObject.BillingAddress.PostCode, webObject.BillingAddress.PostCode);
                Assert.AreEqual(repoObject.BillingAddress.Region, webObject.BillingAddress.Region);

                Assert.AreEqual(repoObject.User.Email, webObject.User.EmailAddress);
                Assert.AreEqual(repoObject.User.Id, webObject.User.Id);
                Assert.AreEqual(repoObject.User.UserName, webObject.User.UserName);

                Assert.AreEqual(repoObject.PurchaseHistory.Count(), webObject.PurchaseHistory.Count());
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CustomerViewModel()
                {
                    BillingAddressId = 123,
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Id = 456,
                    ModifiedDate = DateTime.UtcNow.AddMinutes(1),
                    Surname = "Surname",
                    UserId = 789,
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
                    PurchaseHistory = new List<PurchaseViewModel>(),
                };

                var mapper = new Map.CustomerMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CustomerViewModel()
                {
                    BillingAddressId = 123,
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "EmailAddress",
                    Firstname = "Firstname",
                    Id = 456,
                    ModifiedDate = DateTime.UtcNow.AddMinutes(1),
                    Surname = "Surname",
                    UserId = 789,
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
                    PurchaseHistory = new List<PurchaseViewModel>(),
                };

                Customer toObject = new Customer();

                var mapper = new Map.CustomerMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CustomerViewModel repoObject, Customer webObject)
            {
                Assert.AreEqual(webObject.BillingAddressId, repoObject.BillingAddressId);
                Assert.AreEqual(webObject.CreatedDate, repoObject.CreatedDate);
                Assert.AreEqual(webObject.EmailAddress, repoObject.EmailAddress);
                Assert.AreEqual(webObject.Firstname, repoObject.Firstname);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.ModifiedDate, repoObject.ModifiedDate);
                Assert.AreEqual(webObject.Surname, repoObject.Surname);
                Assert.AreEqual(webObject.UserId, repoObject.UserId);

                Assert.AreEqual(webObject.BillingAddress.AddressLine1, repoObject.BillingAddress.AddressLine1);
                Assert.AreEqual(webObject.BillingAddress.AddressLine2, repoObject.BillingAddress.AddressLine2);
                Assert.AreEqual(webObject.BillingAddress.AddressLine3, repoObject.BillingAddress.AddressLine3);
                Assert.AreEqual(webObject.BillingAddress.City, repoObject.BillingAddress.City);
                Assert.AreEqual(webObject.BillingAddress.Country, repoObject.BillingAddress.Country);
                Assert.AreEqual(webObject.BillingAddress.CountryCode, repoObject.BillingAddress.CountryCode);
                Assert.AreEqual(webObject.BillingAddress.Id, repoObject.BillingAddress.Id);
                Assert.AreEqual(webObject.BillingAddress.PostCode, repoObject.BillingAddress.PostCode);
                Assert.AreEqual(webObject.BillingAddress.Region, repoObject.BillingAddress.Region);
                
                Assert.AreEqual(webObject.PurchaseHistory.Count(), repoObject.PurchaseHistory.Count());
            }
        }
    }
}
