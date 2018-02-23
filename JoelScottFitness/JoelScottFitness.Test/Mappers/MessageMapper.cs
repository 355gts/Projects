using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Dom = JoelScottFitness.Common.Models;
using Map = JoelScottFitness.Services.Mappers;
using Repo = JoelScottFitness.Data.Models;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class MessageMapper
    {
        [TestClass]
        public class WebToRepo
        {
            Dom.MessageViewModel webObject = new Dom.MessageViewModel()
            {
                Id = 123,
                ReceivedDate = new DateTime(2018, 02, 10, 13, 34, 32),
                Responded = true,
                Response = "Response",
                EmailAddress = "EmailAddress",
                JoinMailingList = true,
                Message = "Message",
                Name = "Name",
                Subject = "Subject",
            };

            [TestMethod]
            public void Maps_ToNullObject()
            {
                var mapper = new Map.MessageMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void Maps_ToExisingObject()
            {
                var repoObject = new Repo.Message();

                var mapper = new Map.MessageMapper();

                mapper.Map(webObject, repoObject);

                AssertAreEqual(webObject, repoObject);
            }

            private void AssertAreEqual(Dom.MessageViewModel webObject, Repo.Message repoObject)
            {
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.Responded, repoObject.Responded);
                Assert.AreEqual(webObject.ReceivedDate, repoObject.ReceivedDate);
                Assert.AreEqual(webObject.Response, repoObject.Response);
                Assert.AreEqual(webObject.EmailAddress, repoObject.EmailAddress);
                Assert.AreEqual(webObject.Message, repoObject.Content);
                Assert.AreEqual(webObject.Name, repoObject.Name);
                Assert.AreEqual(webObject.Subject, repoObject.Subject);
            }
        }

        [TestClass]
        public class RepoToWeb
        {
            Repo.Message repoObject = new Repo.Message()
            {
                Id = 123,
                ReceivedDate = new DateTime(2018, 02, 10, 13, 34, 32),
                Responded = true,
                Response = "Response",
                EmailAddress = "EmailAddress",
                Content = "Message",
                Name = "Name",
                Subject = "Subject",
            };

            [TestMethod]
            public void Maps_ToNullObject()
            {
                var mapper = new Map.MessageMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void Maps_ToExisingObject()
            {
                var webObject = new Dom.MessageViewModel();

                var mapper = new Map.MessageMapper();

                mapper.Map(repoObject, webObject);

                AssertAreEqual(repoObject, webObject);
            }

            private void AssertAreEqual(Repo.Message repoObject, Dom.MessageViewModel webObject)
            {
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.Responded, webObject.Responded);
                Assert.AreEqual(webObject.Response, repoObject.Response);
                Assert.AreEqual(repoObject.ReceivedDate, webObject.ReceivedDate);
                Assert.AreEqual(repoObject.EmailAddress, webObject.EmailAddress);
                Assert.AreEqual(repoObject.Content, webObject.Message);
                Assert.AreEqual(repoObject.Name, webObject.Name);
                Assert.AreEqual(repoObject.Subject, webObject.Subject);
            }
        }
    }
}
