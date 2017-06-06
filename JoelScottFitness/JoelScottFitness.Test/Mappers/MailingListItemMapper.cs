using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class MailingListItemMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new MailingListItem()
                {
                    Active = true,
                    Email = "TestEmail",
                    Id = 1,
                };

                var mapper = new Map.MailingListItemMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new MailingListItem()
                {
                    Active = true,
                    Email = "TestEmail",
                    Id = 1,
                };

                MailingListItemViewModel toObject = new MailingListItemViewModel();

                var mapper = new Map.MailingListItemMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(MailingListItem repoObject, MailingListItemViewModel webObject)
            {
                Assert.AreEqual(repoObject.Active, webObject.Active);
                Assert.AreEqual(repoObject.Email, webObject.Email);
                Assert.AreEqual(repoObject.Id, webObject.Id);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new MailingListItemViewModel()
                {
                    Active = true,
                    Email = "TestEmail",
                    Id = 1,
                };

                var mapper = new Map.MailingListItemMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new MailingListItemViewModel()
                {
                    Active = true,
                    Email = "TestEmail",
                    Id = 1,
                };

                MailingListItem toObject = new MailingListItem();

                var mapper = new Map.MailingListItemMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(MailingListItemViewModel webObject, MailingListItem repoObject)
            {
                Assert.AreEqual(repoObject.Active, webObject.Active);
                Assert.AreEqual(repoObject.Email, webObject.Email);
                Assert.AreEqual(repoObject.Id, webObject.Id);
            }
        }
    }
}
