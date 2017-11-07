using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class CreateBlogMapper
    {
        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new CreateBlogViewModel()
                {
                    Content = "Content",
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                };

                var mapper = new Map.CreateBlogMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new CreateBlogViewModel()
                {
                    Content = "Content",
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                };

                Blog toObject = new Blog();

                var mapper = new Map.CreateBlogMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(CreateBlogViewModel webObject, Blog repoObject)
            {
                Assert.AreEqual(webObject.Content, repoObject.Content);
                Assert.IsNotNull(repoObject.CreatedDate);
                Assert.IsNull(repoObject.ModifiedDate);
                Assert.AreEqual(0, repoObject.Id);
                Assert.AreEqual(webObject.ImagePath, repoObject.ImagePath);
                Assert.AreEqual(webObject.SubHeader, repoObject.SubHeader);
                Assert.AreEqual(webObject.Title, repoObject.Title);
                Assert.IsFalse(repoObject.Active);
            }
        }
    }
}
