using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dom = JoelScottFitness.Common.Models;
using Map = JoelScottFitness.Services.Mappers;
using Repo = JoelScottFitness.Data.Models;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateMessageMapper
    {
        [TestClass]
        public class WebToRepo
        {
            Dom.CreateMessageViewModel webObject = new Dom.CreateMessageViewModel()
            {
                EmailAddress = "EmailAddress",
                JoinMailingList = true,
                Message = "Message",
                Name = "Name",
                Subject = "Subject",
            };

            [TestMethod]
            public void Maps_ToNullObject()
            {
                var mapper = new Map.CreateMessageMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void Maps_ToExisingObject()
            {
                var repoObject = new Repo.Message();

                var mapper = new Map.CreateMessageMapper();

                mapper.Map(webObject, repoObject);

                AssertAreEqual(webObject, repoObject);
            }

            private void AssertAreEqual(Dom.CreateMessageViewModel webObject, Repo.Message repoObject)
            {
                Assert.AreEqual(webObject.EmailAddress, repoObject.EmailAddress);
                Assert.AreEqual(webObject.Message, repoObject.Content);
                Assert.AreEqual(webObject.Name, repoObject.Name);
                Assert.AreEqual(webObject.Subject, repoObject.Subject);
                Assert.IsFalse(repoObject.Responded);
                Assert.IsNotNull(repoObject.ReceivedDate);
            }
        }
    }
}
