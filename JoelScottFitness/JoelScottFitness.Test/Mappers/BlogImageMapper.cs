using JoelScottFitness.Common.Enumerations;
using JoelScottFitness.Common.Models;
using JoelScottFitness.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Map = JoelScottFitness.Services.Mappers;

namespace JoelScottFitness.Test.Mappers
{
    [TestClass]
    public class BlogImageMapper
    {
        [TestClass]
        public class RepoToWeb
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var repoObject = new BlogImage()
                {
                    BlogId = 123,
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    Id = 333,
                    ImagePath = "ImagePath",
                };

                var mapper = new Map.BlogImageMapper();

                var result = mapper.Map(repoObject);

                AssertAreEqual(repoObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var repoObject = new BlogImage()
                {
                    BlogId = 123,
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    Id = 333,
                    ImagePath = "ImagePath",
                };
    
                BlogImageViewModel toObject = new BlogImageViewModel();

                var mapper = new Map.BlogImageMapper();

                mapper.Map(repoObject, toObject);

                AssertAreEqual(repoObject, toObject);
            }

            private void AssertAreEqual(BlogImage repoObject, BlogImageViewModel webObject)
            {
                Assert.AreEqual(repoObject.BlogId, webObject.BlogId);
                Assert.AreEqual(repoObject.Caption, webObject.Caption);
                Assert.AreEqual(repoObject.CaptionColour, webObject.CaptionColour);
                Assert.AreEqual(repoObject.CaptionTitle, webObject.CaptionTitle);
                Assert.AreEqual(repoObject.Id, webObject.Id);
                Assert.AreEqual(repoObject.ImagePath, webObject.ImagePath);
            }
        }

        [TestClass]
        public class WebToRepo
        {
            [TestMethod]
            public void FromObject_ToNullObject()
            {
                var webObject = new BlogImageViewModel()
                {
                    BlogId = 123,
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    Id = 333,
                    ImagePath = "ImagePath",
                };

                var mapper = new Map.BlogImageMapper();

                var result = mapper.Map(webObject);

                AssertAreEqual(webObject, result);
            }

            [TestMethod]
            public void FromObject_ToObject()
            {
                var webObject = new BlogImageViewModel()
                {
                    BlogId = 123,
                    Caption = "Caption",
                    CaptionColour = BlogCaptionTextColour.White,
                    CaptionTitle = "CaptionTitle",
                    Id = 333,
                    ImagePath = "ImagePath",
                };

                BlogImage toObject = new BlogImage();

                var mapper = new Map.BlogImageMapper();

                mapper.Map(webObject, toObject);

                AssertAreEqual(webObject, toObject);
            }

            private void AssertAreEqual(BlogImageViewModel webObject, BlogImage repoObject)
            {
                Assert.AreEqual(webObject.BlogId, repoObject.BlogId);
                Assert.AreEqual(webObject.Caption, repoObject.Caption);
                Assert.AreEqual(webObject.CaptionColour, repoObject.CaptionColour);
                Assert.AreEqual(webObject.CaptionTitle, repoObject.CaptionTitle);
                Assert.AreEqual(webObject.Id, repoObject.Id);
                Assert.AreEqual(webObject.ImagePath, repoObject.ImagePath);
            }
        }
    }
}
