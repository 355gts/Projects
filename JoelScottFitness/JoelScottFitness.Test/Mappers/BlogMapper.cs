using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class BlogMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new Blog()
                {
                    Content = "Content",
                    CreatedDate = new DateTime(2017, 05, 02, 14, 14, 15),
                    Id = 123,
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    Active = true,
                    ModifiedDate = new DateTime(2017, 06, 02, 14, 14, 15),
                };

                var mapper = new Map.BlogMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new Blog()
                {
                    Content = "Content",
                    CreatedDate = new DateTime(2017, 05, 02, 14, 14, 15),
                    Id = 123,
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    Active = true,
                    ModifiedDate = new DateTime(2017, 06, 02, 14, 14, 15),
                };

                BlogViewModel toObject = new BlogViewModel();

                var mapper = new Map.BlogMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(Blog repoObject, BlogViewModel webObject)
            {
                Assert.AreEqual(repoObject.Content, webObject.Content);
                Assert.AreEqual(repoObject.CreatedDate, webObject.CreatedDate);
                Assert.AreEqual(repoObject.ModifiedDate, webObject.ModifiedDate);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
                Assert.AreEqual(repoObject.SubHeader, webObject.SubHeader);
                Assert.AreEqual(repoObject.Title, webObject.Title);
                Assert.AreEqual(repoObject.Active, webObject.Active);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new BlogViewModel()
                {
                    Content = "Content",
                    CreatedDate = new DateTime(2017, 05, 02, 14, 14, 15),
                    Id = 123,
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    Active = true,
                };

                var mapper = new Map.BlogMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new BlogViewModel()
                {
                    Content = "Content",
                    CreatedDate = new DateTime(2017, 05, 02, 14, 14, 15),
                    Id = 123,
                    ImagePath = "ImagePath",
                    SubHeader = "SubHeader",
                    Title = "Title",
                    Active = true,
                };

                Blog toObject = new Blog();

                var mapper = new Map.BlogMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(BlogViewModel webObject, Blog repoObject)
            {
                Assert.AreEqual(webObject.Content, repoObject.Content);
                Assert.AreEqual(webObject.CreatedDate, repoObject.CreatedDate);
                Assert.IsNotNull(repoObject.ModifiedDate);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.ImagePath, repoObject.ImagePath);
                Assert.AreEqual(webObject.SubHeader, repoObject.SubHeader);
                Assert.AreEqual(webObject.Title, repoObject.Title);
                Assert.AreEqual(repoObject.Active, webObject.Active);
            }
        }
    }
}
